namespace MisteryBlazor.Enums
{
    /// <summary>
    /// Accepted 为已被接收者同意<br/>
    /// Requesting 为请求中，即没通过也没拒绝<br/>
    /// Rejected 为已被接收者拒绝<br/>
    /// Deleted 为已被某一方删除<br/>
    /// Banned 为已被某一方拉黑
    /// </summary>
    public enum RelationStatus
    {
        Accepted,
        Requesting,
        Rejected,
        DeletedFromRequester,
        DeletedFromReceiver,
        BannedFromRequester,
        BannedFromReceiver,
    }
}
