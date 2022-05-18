namespace MisteryBlazor.Enums
{
    /// <summary>
    /// 群成员状态枚举<br/>
    /// Accepted 为已通过 <br/>
    /// Requesting 为申请中，既没通过也没拒绝<br/>
    /// Rejected 为已拒绝<br/>
    /// Quit 为已退出<br/>
    /// Kicked 为已被踢<br/>
    /// Banned 为已被该群拉黑
    /// </summary>
    public enum GroupMemberStatus
    {
        Accepted,
        Requesting,
        Rejected,
        Quit,
        Kicked,
        Banned,
    }
}
