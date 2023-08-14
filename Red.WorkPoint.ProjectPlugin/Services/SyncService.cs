using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.MSProject;
using Red.WorkPoint.ApiClient;
using Red.WorkPoint.ProjectPlugin.Resources;
using Exception = System.Exception;
using Project = Red.WorkPoint.ApiClient.Project;
using Resource = Microsoft.Office.Interop.MSProject.Resource;
using Task = System.Threading.Tasks.Task;
using MSProjectTask = Microsoft.Office.Interop.MSProject.Task;

namespace Red.WorkPoint.ProjectPlugin.Services
{
    /// <summary>
    /// Sync service.
    /// </summary>
    public class SyncService
    {
        private readonly ListBox _log;
        private List<UserInfo> Users { get; set; }
        private const int PageSize = 100;

        public SyncService(ListBox log)
        {
            _log = log;
        }

        /// <summary>
        /// Checks project resources.
        /// </summary>
        /// <returns>True if check was successful.</returns>
        public bool CheckProjectFile()
        {
            // Project team checking.
            var emails = new List<string>();

            foreach (Resource resource in Context.Project.Resources)
            {
                if (resource != null && !string.IsNullOrEmpty(resource.EMailAddress)) emails.Add(resource.EMailAddress);
            }

            if (emails.Count == emails.Distinct().Count()) return true;

            AddLog(LocalStrings.MessageTeamHasDuplicateEmails);
            AddLog(LocalStrings.MessageSynchronizationIsNotPossible);

            return false;
        }

        /// <summary>
        /// Publishes project to Timetta.
        /// </summary>
        /// <param name="billingTypeId">Billing type Id.</param>
        /// <param name="userId">User Id.</param>
        /// <returns>Published project.</returns>
        public async Task<Project> PublishProject(Guid billingTypeId, Guid userId)
        {
            DataService.AccessToken = Context.AccessToken;

            var project = new Project
            {
                Name = Context.Application.ActiveProject.Project,
                StartDate = Context.Project.ProjectStart,
                EndDate = Context.Project.ProjectFinish,
                Description = Context.Project.Notes,
                BillingTypeId = billingTypeId,
                StateId = ProjectState.Tentative.Id,
                ManagerId = userId
            };

            AddLog(LocalStrings.MessageProjectIsCreating);
            var newProject = await DataService
                .Collection("Projects")
                .Insert(project);

            return newProject;
        }

        /// <summary>
        /// Updates project.
        /// </summary>
        /// <returns>Updated project.</returns>
        public async Task<Project> UpdateProject()
        {
            DataService.AccessToken = Context.AccessToken;

            var project = await DataService
                .Collection("Projects")
                .Entity(Context.ProjectId)
                .Get<Project>();

            project.Name = Context.Application.ActiveProject.Project;
            project.StartDate = Context.Project.ProjectStart;
            project.EndDate = Context.Project.ProjectFinish;
            project.Description = Context.Project.Notes;

            AddLog(LocalStrings.MessageProjectIsUpdating);
            var newProject = await DataService.Collection("Projects").Entity(Context.ProjectId).Update(project);

            return newProject;
        }

        /// <summary>
        /// Returns default role Id.
        /// </summary>
        /// <returns>Default Role id if exists.</returns>
        private async Task<Guid?> GetDefaultRoleId()
        {
            var roles = await DataService
                .Collection("Roles")
                .Query<NamedEntity>(new ODataParams
                {
                    Filter = $"isDefault eq true"
                });

            if (roles.Count != 1)
            {
                return null;
            }

            return roles[0].Id;
        }

        /// <summary>
        /// Synchronizes team.
        /// </summary>
        public async Task SyncTeam()
        {
            try
            {
                AddLog(LocalStrings.MessageTeamSync);
                DataService.AccessToken = Context.AccessToken;

                var defaultRoleId = await GetDefaultRoleId();

                if (!defaultRoleId.HasValue)
                {
                    AddLog(string.Format(LocalStrings.MessageThereIsNoDefaultRole));
                    return;
                }

                var teamMembers = await DataService
                    .Collection("Projects")
                    .Entity(Context.ProjectId)
                    .Collection("ProjectTeamMembers")
                    .Query<ProjectTeamMember>(new ODataParams
                    {
                        Filter = $"resource/resourceType eq '{ResourceType.User}'",
                        Expand = "RoleAssignments"
                    });

                foreach (Resource resource in Context.Project.Resources)
                {
                    // Empty row at resource list.
                    if (resource == null) continue;

                    var email = resource.EMailAddress.ToLower();

                    if (string.IsNullOrEmpty(email))
                    {
                        continue;
                    }

                    var user = await GetUserByEmail(email);

                    // If user is not exist skip.
                    if (user == null)
                    {
                        AddLog(string.Format(LocalStrings.MessageUserEmailNotFound, resource.Name));
                        continue;
                    }

                    var teamMember = teamMembers.FirstOrDefault(member => member.ResourceId.Equals(user.Id));

                    // A team member is already in place and active.
                    if (teamMember?.IsActive == true)
                    {
                        continue;
                    }

                    if (teamMember != null && !teamMember.IsActive)
                    {
                        // Activate team member.
                        await DataService.Collection("ProjectTeamMembers")
                            .Entity(teamMember.Id)
                            .Patch(new { IsActive = true });
                    }
                    else
                    {
                        // Create team member.
                        AddLog(string.Format(LocalStrings.MessageAddingTeamMember, user.Name));
                        await DataService.Collection("ProjectTeamMembers").Insert(new ProjectTeamMember
                        {
                            ResourceId = user.Id,
                            ProjectId = Context.ProjectId,
                            RoleAssignments = new List<ProjectTeamMemberRole>
                            {
                                new ProjectTeamMemberRole
                                {
                                    RoleId = defaultRoleId.Value
                                }
                            }
                        });
                    }
                }

                // Clear project team members.
                foreach (var teamMember in teamMembers)
                {
                    var user = (await DataService.Collection("Users")
                        .Query<UserInfo>(new ODataParams
                        {
                            Select = "id, name, email",
                            Filter = $"id eq {teamMember.ResourceId}"
                        }))[0];


                    var exists = false;

                    foreach (Resource resource in Context.Project.Resources)
                    {
                        if (resource == null) continue;

                        if (resource.EMailAddress.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase))
                        {
                            exists = true;
                        }
                    }

                    if (!exists)
                    {
                        AddLog(string.Format(LocalStrings.MessageRemovingTeamMember, user.Name));
                        // Включить.
                        await DataService.Collection("ProjectTeamMembers")
                            .Entity(teamMember.Id)
                            .Patch(new { IsActive = false });
                    }
                }
            }

            catch (Exception ex)
            {
                AddLog(LocalStrings.ErrorOccured);
                AddLog(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Synchronizes tasks.
        /// </summary>
        /// <exception cref="Exception">Task exception.</exception>
        public async Task SyncTasks()
        {
            DataService.AccessToken = Context.AccessToken;
            AddLog(LocalStrings.MessageTaskSync);
            var removedWpTaskIds = new List<Guid>();

            try
            {
                // Loading of existing tasks
                var wpTasks = new List<ProjectTask>();

                async Task LoadTasksPage(int page = 0)
                {
                    var tasksPage = await DataService.Collection("ProjectTasks")
                        .Query<ProjectTask>(new ODataParams
                        {
                            Top = PageSize,
                            Skip = page * PageSize,
                            Filter = $"ProjectId eq {Context.ProjectId}"
                        });
                    wpTasks.AddRange(tasksPage);
                    if (tasksPage.Count == PageSize)
                    {
                        await LoadTasksPage(page + 1);
                    }
                }

                await LoadTasksPage();

                var wpMainTask = wpTasks.SingleOrDefault(t => !t.LeadTaskId.HasValue);

                // Loading project team members.
                var wpTeamMembers = await DataService
                    .Collection("Projects")
                    .Entity(Context.ProjectId)
                    .Collection("ProjectTeamMembers")
                    .Query<ProjectTeamMember>(new ODataParams
                    {
                        Filter = $"resource/resourceType eq '{ResourceType.User}'",
                        Expand = "resource($select=id,name)"
                    });


                var tasks = Context.Project.Tasks;

                // Check the task IDs in the file for duplicates. If there are any, remove them.
                List<MSProjectTask> GetLocalDuplicateTasksByWpId(Guid wpId)
                {
                    var duplicates = new List<MSProjectTask>();
                    foreach (MSProjectTask task in tasks)
                    {
                        if (task == null) continue;
                        var taskIdValue = task.GetField(PjField.pjTaskText30);
                        if (!string.IsNullOrEmpty(taskIdValue))
                        {
                            Guid? taskId = Guid.Parse(taskIdValue);
                            if (taskId.Equals(wpId))
                            {
                                duplicates.Add(task);
                            }
                        }
                    }

                    return duplicates;
                }

                foreach (MSProjectTask task in tasks)
                {
                    if (task == null) continue;

                    var taskIdValue = task.GetField(PjField.pjTaskText30);
                    if (!string.IsNullOrEmpty(taskIdValue))
                    {
                        Guid? taskId = Guid.Parse(taskIdValue);
                        var duplicates = GetLocalDuplicateTasksByWpId(taskId.Value);
                        duplicates = duplicates.OrderBy(d => (DateTime)d.Created).ToList();

                        var index = 0;
                        foreach (var duplicate in duplicates)
                        {
                            // Delete the ID of all tasks except the earliest by creation date.
                            if (index != 0)
                            {
                                duplicate.SetField(PjField.pjTaskText30, string.Empty);
                            }

                            index++;
                        }
                    }
                }


                // Adding non-existent tasks.
                foreach (MSProjectTask task in tasks)
                {
                    if (task == null) continue;

                    Guid? taskId = null;
                    var taskIdValue = task.GetField(PjField.pjTaskText30);
                    if (!string.IsNullOrEmpty(taskIdValue))
                    {
                        taskId = Guid.Parse(taskIdValue);
                    }

                    if (!taskId.HasValue || !wpTasks.Exists(t => t.Id.Equals(taskId)))
                    {
                        var wpTaskNew = new ProjectTask
                        {
                            ProjectId = Context.ProjectId,
                            AllowTimeEntry = true,
                            Name = task.Name,
                            ProjectTaskAssignments = new List<ProjectTaskAssignment>()
                        };
                        wpTasks.Add(wpTaskNew);
                        task.SetField(PjField.pjTaskText30, wpTaskNew.Id.ToString());
                    }
                }

                // Close tasks that are no longer in Project.
                foreach (var wpTask in wpTasks)
                {
                    if (!wpTask.LeadTaskId.HasValue)
                        continue;

                    if (GetLocalTaskById(wpTask.Id) != null) continue;

                    wpTask.IsActive = false;
                    removedWpTaskIds.Add(wpTask.Id);
                }

                // Updating the structure and properties.
                foreach (MSProjectTask task in tasks)
                {
                    if (task == null)
                        continue;

                    var taskIdValue = task.GetField(PjField.pjTaskText30);
                    var task2Id = Guid.Parse(taskIdValue);

                    var wpTask = wpTasks.SingleOrDefault(t => t.Id.Equals(task2Id));

                    if (wpTask == null)
                        throw new Exception("Error has occurred on task update. Task not found by stored Id.");

                    var parentTask = task.OutlineParent;
                    if (parentTask.OutlineLevel > 0)
                    {
                        Guid? parentTaskId = null;
                        var parentTaskIdValue = parentTask.GetField(PjField.pjTaskText30);
                        if (!string.IsNullOrEmpty(parentTaskIdValue))
                            parentTaskId = Guid.Parse(parentTaskIdValue);

                        wpTask.LeadTaskId = parentTaskId;
                    }
                    else
                    {
                        wpTask.LeadTaskId = wpMainTask.Id;
                    }

                    var index = int.Parse(task.OutlineNumber.Split('.').Last());

                    wpTask.Number = index - 1;
                    wpTask.Name = task.Name;
                    wpTask.Description = task.Notes;
                    wpTask.StartDate = task.Start;
                    wpTask.EndDate = task.Finish;
                    wpTask.IsActive = task.Status != PjStatusType.pjComplete;
                    wpTask.ProjectTaskAssignments = new List<ProjectTaskAssignment>();

                    if (!task.Summary)
                    {
                        wpTask.AllowTimeEntry = true;

                        // Updating Assignments.
                        var hasAssignments = false;
                        var hoursByAssignments = 0m;

                        foreach (Assignment assignment in task.Assignments)
                        {
                            var email = assignment.Resource.EMailAddress;
                            var user = await GetUserByEmail(email);

                            if (user == null)
                            {
                                continue;
                            }

                            var teamMember = wpTeamMembers.Find(m => m.ResourceId == user.Id);

                            if (wpTask.ProjectTaskAssignments.Any(a =>
                                    a.ProjectTeamMemberId.Equals(teamMember.Id))) continue;

                            var hours = (decimal?)assignment.Work / 60;
                            hoursByAssignments += hours ?? 0;
                            hasAssignments = true;
                            wpTask.ProjectTaskAssignments.Add(new ProjectTaskAssignment
                            {
                                ProjectTaskId = wpTask.Id,
                                ProjectTeamMemberId = teamMember.Id
                            });
                        }

                        if (!hasAssignments)
                        {
                            wpTask.EstimatedDuration = (decimal?)task.Work / 60;
                        }
                        else
                        {
                            if (hoursByAssignments > 0) wpTask.EstimatedDuration = hoursByAssignments;
                        }
                    }
                    else
                    {
                        wpTask.AllowTimeEntry = false;
                        wpTask.EstimatedDuration = null;
                    }
                }

                // Collect all non-existent tasks at the end of the level.
                var removedWpTasks = wpTasks.Where(t => removedWpTaskIds.Any(rt => rt == t.Id)).OrderBy(t => t.Number)
                    .ToList();
                foreach (var wpTask in removedWpTasks)
                {
                    var maxNumber = wpTasks.Where(t => t.LeadTaskId == wpTask.LeadTaskId
                                                       && t.Id != wpTask.Id
                                                       && removedWpTaskIds.All(rt => rt != t.Id))
                        .Select(t => t.Number)
                        .DefaultIfEmpty(-1)
                        .Max();

                    removedWpTaskIds.Remove(wpTask.Id);
                    wpTask.Number = maxNumber + 1;
                }

                var dto = new
                {
                    ProjectTasks = wpTasks.Select(wpTask => new
                    {
                        wpTask.Id,
                        Context.ProjectId,
                        wpTask.Name,
                        wpTask.IsActive,
                        wpTask.AllowTimeEntry,
                        wpTask.LeadTaskId,
                        wpTask.RowVersion,
                        wpTask.Number,
                        wpTask.StartDate,
                        wpTask.EndDate,
                        wpTask.Description,
                        wpTask.EstimatedDuration,
                        wpTask.DateValue1,
                        wpTask.DateValue2,
                        wpTask.DateValue3,
                        wpTask.DateValue4,
                        wpTask.DateValue5,
                        wpTask.DecimalValue1,
                        wpTask.DecimalValue2,
                        wpTask.DecimalValue3,
                        wpTask.DecimalValue4,
                        wpTask.DecimalValue5,
                        wpTask.LookupValue1Id,
                        wpTask.LookupValue2Id,
                        wpTask.LookupValue3Id,
                        wpTask.LookupValue4Id,
                        wpTask.LookupValue5Id,
                        wpTask.StringValue1,
                        wpTask.StringValue2,
                        wpTask.StringValue3,
                        wpTask.StringValue4,
                        wpTask.StringValue5,

                        ProjectTaskAssignments =
                            wpTask.ProjectTaskAssignments?.ToList() ?? new List<ProjectTaskAssignment>()
                    })
                };


                var leadTask = dto.ProjectTasks.SingleOrDefault(t => !t.LeadTaskId.HasValue);
                var mainTaskId = leadTask.Id;
                var processedTaskIds = new List<Guid> { leadTask.Id };

                void CheckSubTasks(Guid leadTaskId)
                {
                    var number = 0;
                    var subTasks = dto.ProjectTasks
                        .Where(t => t.LeadTaskId == leadTaskId)
                        .OrderBy(p => p.Number)
                        .ToList();

                    foreach (var subTask in subTasks)
                    {
                        // Check number and cyclicality.
                        if (subTask.Number != number || processedTaskIds.Contains(subTask.Id))
                            throw new Exception("Tasks structure is corrupted! Saving canceled.");
                        processedTaskIds.Add(subTask.Id);

                        number++;
                        CheckSubTasks(subTask.Id);
                    }
                }

                CheckSubTasks(mainTaskId);


                await DataService.Collection("Projects")
                    .Entity(Context.ProjectId)
                    .Action("WP.UpdateProjectTasks")
                    .Execute(dto);

                Context.Application.FileSave();
            }
            catch (Exception ex)
            {
                AddLog(LocalStrings.ErrorOccured);
                AddLog(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Synchronizes(exports) resource plan.
        /// </summary>
        public async Task SyncResourcePlan()
        {
            DataService.AccessToken = Context.AccessToken;
            AddLog(LocalStrings.MessageResourcePlanSync);

            try
            {
                var wpEntries = new List<ResourcePlanDto>();

                var defaultRoleId = await GetDefaultRoleId();

                if (!defaultRoleId.HasValue)
                {
                    AddLog(string.Format(LocalStrings.MessageThereIsNoDefaultRole));
                    return;
                }


                // Loading team members.
                var wpTeamMembers = await DataService
                    .Collection("Projects")
                    .Entity(Context.ProjectId)
                    .Collection("ProjectTeamMembers")
                    .Query<ProjectTeamMember>(new ODataParams
                    {
                        Filter = $"resource/resourceType eq '{ResourceType.User}'",
                        Expand = "resource($select=id,name)"
                    });

                // Clearing resource plan.
                await DataService
                    .Collection("Projects")
                    .Entity(Context.ProjectId)
                    .Action("WP.ClearResourcePlan")
                    .Execute();

                var startDate = (DateTime)Context.Project.ProjectStart;
                var endDate = (DateTime)Context.Project.ProjectFinish;

                if (endDate < startDate)
                {
                    AddLog(LocalStrings.MessageStartDateIsAfterEndDate);
                    return;
                }

                AddLog(string.Format(LocalStrings.SyncPeriodTemplate, startDate.ToShortDateString(),
                    endDate.ToShortDateString()));


                var tasks = Context.Project.Tasks;

                // Add non-existent tasks.
                foreach (MSProjectTask task in tasks)
                {
                    if (task == null) continue;

                    // Get task Id.
                    var taskIdValue = task.GetField(PjField.pjTaskText30);
                    if (string.IsNullOrEmpty(taskIdValue)) continue;
                    var taskId = Guid.Parse(taskIdValue);

                    // Unique check.
                    var emails = new List<string>();
                    foreach (Assignment assignment in task.Assignments)
                    {
                        if (!string.IsNullOrEmpty(assignment.Resource.EMailAddress))
                            emails.Add(assignment.Resource.EMailAddress);
                    }

                    if (emails.Distinct().Count() != emails.Count)
                    {
                        AddLog(string.Format(LocalStrings.MessageDuplicateEmailsInAssignmentsByTask, task.Name));
                    }

                    foreach (Assignment assignment in task.Assignments)
                    {
                        // Get team member Id.
                        var email = assignment.Resource.EMailAddress.ToLower();

                        var user = await GetUserByEmail(email);
                        if (user == null || wpTeamMembers.All(t => t.Resource.Id != user.Id))
                        {
                            continue;
                        }

                        var teamUser = await GetUserByEmail(email);

                        if (teamUser == null) continue;

                        var teamMember = wpTeamMembers.SingleOrDefault(m => m.ResourceId == teamUser.Id);

                        if (teamMember == null) continue;

                        var timeScaleData = assignment.TimeScaleData(startDate, endDate,
                            PjAssignmentTimescaledData.pjAssignmentTimescaledWork,
                            PjTimescaleUnit.pjTimescaleDays);

                        foreach (TimeScaleValue timeScaleValue in timeScaleData)
                        {
                            var hours = (decimal)((timeScaleValue.Value as double? ?? 0) / 60);

                            // System limitation.
                            if (hours > 99) continue;

                            // Do not send empty values.
                            if (hours == 0) continue;

                            wpEntries.Add(new ResourcePlanDto
                            {
                                TeamMemberId = teamMember.Id,
                                RoleId = defaultRoleId.Value,
                                TaskId = taskId,
                                Hours = hours,
                                Date = (DateTime)timeScaleValue.StartDate
                            });
                        }
                    }
                }


                // Save data to Timetta.
                var data = new
                {
                    scale = PlanningScale.Day.ToString(),
                    entries = wpEntries
                };

                await DataService
                    .Collection("Projects")
                    .Entity(Context.ProjectId)
                    .Action("WP.UpdateResourcePlan")
                    .Execute(data);
            }
            catch (Exception ex)
            {
                AddLog(LocalStrings.ErrorOccured);
                AddLog(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Imports actual hours.
        /// </summary>
        public async Task SyncActualHours()
        {
            if (!Properties.Settings.Default.ImportActualHours)
            {
                return;
            }

            try
            {
                DataService.AccessToken = Context.AccessToken;
                AddLog(LocalStrings.MessageActualSync);

                var data = await DataService.Model()
                    .Function("GetAllocationData")
                    .Query<AllocationData>(new ODataParams
                    {
                        Filter = $"project/id eq {Context.ProjectId}"
                    });

                foreach (MSProjectTask task in Context.Project.Tasks)
                {
                    if (task == null || task.Summary || !task?.Active)
                        continue;

                    var taskIdValue = task.GetField(PjField.pjTaskText30);

                    if (string.IsNullOrEmpty(taskIdValue))
                        continue;


                    var taskId = Guid.Parse(taskIdValue);

                    var entriesByTask = data.Where(dayData => dayData.ProjectTask.Id == taskId).ToList();

                    foreach (Assignment assignment in task.Assignments)
                    {
                        var entries = entriesByTask.Where(e => e.User.Email
                                .Equals(assignment.Resource.EMailAddress, StringComparison.InvariantCultureIgnoreCase))
                            .ToList();

                        var startDate = (DateTime)Context.Project.ProjectStart;
                        var endDate = (DateTime)Context.Project.ProjectFinish;

                        if (entries.Any())
                        {
                            startDate = new DateTime(Math.Min(entries.Min(e => e.Date).Ticks, startDate.Ticks));
                            endDate = new DateTime(Math.Max(entries.Max(e => e.Date).Ticks, endDate.Ticks));
                        }

                        if (assignment.ActualStart is DateTime actualStart)
                        {
                            startDate = new DateTime(Math.Min(actualStart.Ticks, startDate.Ticks));
                        }

                        if (assignment.ActualFinish is DateTime actualFinish)
                        {
                            endDate = new DateTime(Math.Max(actualFinish.Ticks, endDate.Ticks));
                        }

                        if (assignment.Start is DateTime start)
                        {
                            startDate = new DateTime(Math.Min(start.Ticks, startDate.Ticks));
                        }

                        if (assignment.Finish is DateTime finish)
                        {
                            endDate = new DateTime(Math.Max(finish.Ticks, endDate.Ticks));
                        }

                        var groupedEntries = entries.GroupBy(dayData => dayData.Date, dayData => dayData).Select(e =>
                            new
                            {
                                Date = e.Key,
                                Hours = e.Sum(t => t.Hours)
                            }).ToList();

                        foreach (var day in EachDay(startDate, endDate))
                        {
                            var groupedEntry = groupedEntries.SingleOrDefault(e => e.Date.Equals(day));
                            var duration = groupedEntry?.Hours;

                            var timePhasedData = assignment.TimeScaleData(day,
                                day.AddDays(1),
                                PjAssignmentTimescaledData.pjAssignmentTimescaledActualWork,
                                PjTimescaleUnit.pjTimescaleDays);


                            var savedDuration = timePhasedData[1].Value is double ? timePhasedData[1].Value : 0;

                            if (!duration.HasValue)
                            {
                                if (savedDuration != 0)
                                {
                                    timePhasedData[1].Delete();
                                }
                            }
                            else
                            {
                                var durationInMinutes = duration.Value * 60;
                                if (savedDuration != durationInMinutes)
                                {
                                    timePhasedData[1].Value = durationInMinutes;
                                }
                            }
                        }
                    }
                }

                Context.Application.FileSave();
            }
            catch (Exception ex)
            {
                AddLog(LocalStrings.ErrorOccured);
                AddLog(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets User by email.
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User info.</returns>
        private async Task<UserInfo> GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;
            Users = Users ?? await DataService.Collection("Users")
                .Query<UserInfo>(new ODataParams { Filter = $"isActive eq true" });

            return Users.SingleOrDefault(e => e.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Gets local task by Id.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <returns>MSProject task.</returns>
        private MSProjectTask GetLocalTaskById(Guid id)
        {
            foreach (MSProjectTask task in Context.Project.Tasks)
            {
                if (task == null) continue;

                var taskIdValue = task.GetField(PjField.pjTaskText30);
                var taskId = Guid.Parse(taskIdValue);

                if (taskId.Equals(id))
                {
                    return task;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds log.
        /// </summary>
        /// <param name="text">Log message.</param>
        private void AddLog(string text)
        {
            _log.Items.Add(text);
        }

        /// <summary>
        /// Returns an enumeration of dates for the given period.
        /// </summary>
        /// <param name="from">Start date of the period.</param>
        /// <param name="thru">Finish date of the period</param>
        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}