﻿using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.GroupsModel
{
    public class GroupMember
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomPermissionRoleId { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public string GroupMemberId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
