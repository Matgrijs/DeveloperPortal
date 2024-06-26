﻿using System.Text.Json.Serialization;

namespace DeveloperPortal.Models.Poker;

public class PokerVote
{
    [JsonPropertyName("id")] public Guid Id { get; set; }

    [JsonPropertyName("username")] public required string? Username { get; set; }

    [JsonPropertyName("vote")] public required string Vote { get; set; }
}