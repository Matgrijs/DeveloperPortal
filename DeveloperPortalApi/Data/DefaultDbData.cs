namespace DeveloperPortalApi.Entities;

public static class DefaultDbData
{
    #region ChatMessages

    private static readonly ChatMessage Message1 = new()
    {
        Id = Guid.NewGuid(),
        Username = "Matthijs",
        auth0Id = "auth0|661834a129d6402ebd9baa0c",
        Message = "Hey developer, leuk dat je er bent!",
        MessageTime = new DateTime(2024, 4, 7, 16, 0, 0)
    };

    private static readonly ChatMessage Message2 = new()
    {
        Id = Guid.NewGuid(),
        Username = "Developer",
        auth0Id = "auth0|661834a129d6402ebd9baa0c",
        Message = "Hey Matthijs, fijn dat ik er ben!",
        MessageTime = new DateTime(2024, 4, 7, 16, 0, 0)
    };

    #endregion
    public static readonly List<ChatMessage> ChatMessages = [Message1, Message2];
    
    #region Notes

    private static readonly Note Note1 = new()
    {
        Id = Guid.NewGuid(),
        Username = "Matthijs",
        auth0Id = "auth0|661834a129d6402ebd9baa0c",
        Content = "Applicatie afmaken!",
    };

    private static readonly Note Note2 = new()
    {
        Id = Guid.NewGuid(),
        Username = "Developer",
        auth0Id = "auth0|661834a129d6402ebd9baa0c",
        Content = "Sell the application!",
    };

    #endregion
    public static readonly List<Note> Notes = [Note1, Note2];
    
    #region PokerVotes

    private static readonly PokerVote Vote1 = new()
    {
        Id = Guid.NewGuid(),
        Username = "Matthijs Meijboom",
        auth0Id = "auth0|661834a129d6402ebd9baa0c",
        Vote = "\u2615"
    };

    private static readonly PokerVote Vote2 = new()
    {
        Id = Guid.NewGuid(),
        Username = "Developer",
        auth0Id = "auth0|661834a129d6402ebd9baa0c",
        Vote = "8"
    };

    #endregion
    public static readonly List<PokerVote> PokerVotes = [Vote1, Vote2];
    
}
