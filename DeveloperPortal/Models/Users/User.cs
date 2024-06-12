﻿using DeveloperPortal.Models.Poker;

namespace DeveloperPortal.Models.Users;

public class User
{
    public string Name { get; set; }
    public string Auth0Id { get; set; }
    public PokerVote Vote { get; set; }
}