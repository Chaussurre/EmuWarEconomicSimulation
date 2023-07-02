using CombatSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinator.Standard
{
    public class PlayerManager : Cardinator.PlayerManager
    {
        List<Player> Players = new();
        public CardManager CardManager;

        public Player ActivePlayer => Players[0];

        public DataWatcher<PlayData> PlayWatcher;
        public DataWatcher<AttackData> AttackWatcher;

        public void Awake()
        {
            for (int i = 0; i < PlayerCount; i++)
                Players.Add(new(i, this));
        }

        public Player GetPlayer(int i)
        {
            return Players[i];
        }

        public bool PlayCard(Player player, int CardID)
        {
            var data = PlayData.Create(CardID, player, CardManager);
            return DoAction(data, PlayWatcher, CardManager);
        }

        public bool AttackCard(Player player, int AttackerID, int TargetID)
        {
            var data = AttackData.Create(player, AttackerID, TargetID, CardManager);
            return DoAction(data, AttackWatcher, CardManager);
        }

        public bool IsPlayCardValid(Player player, int CardID)
        {
            var data = PlayData.Create(CardID, player, CardManager);
            return IsActionValid(data, PlayWatcher);
        }

        public bool IsAttackCardValid(Player player, int AttackerID, int TargetID)
        {
            var data = AttackData.Create(player, AttackerID, TargetID, CardManager);
            return IsActionValid(data, AttackWatcher);
        }

        public void PlayerDraw(int player)
        {
            CardManager.ActionsManager.AddAction(ActionDrawWatcher.Draw(Players[player], 1));
        }

        public struct PlayData : IActionData
        {
            public bool isActionAllowed;
            public int CardID;
            public Player player;

            public static PlayData Create<TCardData>(int CardID, Player player, CardManager<TCardData> cardManager) where TCardData : struct
            {
                var card = cardManager.GetCardInstance(CardID);

                return new()
                {
                    CardID = CardID,
                    player = player,
                    isActionAllowed = card?.VisibleMask * player.playerID ?? false,
                };
            }

            public bool IsActionAllowed() => isActionAllowed;

            public void DoAction<TCardData>(CardManager<TCardData> cardManager) where TCardData : struct
            {
                var actionData = ActionPlayWatcher.Play(CardID);
                cardManager.ActionsManager.AddAction(actionData);
            }
        }

        public struct AttackData : IActionData
        {
            public bool isActionAllowed;
            public int AttackerID;
            public int TargetID;
            public Player player;


            public static AttackData Create<TCardData>(Player player, int AttackerID, int TargetID, CardManager<TCardData> cardManager)
                where TCardData : struct
            {
                var cardAttacker = cardManager.GetCardInstance(AttackerID);
                var cardTarget = cardManager.GetCardInstance(TargetID);

                bool validAttacker = cardAttacker?.VisibleMask * player.playerID ?? false;

                return new()
                {
                    AttackerID = AttackerID,
                    TargetID = TargetID,
                    player = player,
                    isActionAllowed = validAttacker && cardTarget.HasValue,
                };
            }

            public bool IsActionAllowed() => isActionAllowed;

            public void DoAction<TCardData>(CardManager<TCardData> cardManager) where TCardData : struct
            {
                var actionData = ActionStrikeWatcher.Fight(AttackerID, TargetID);
                cardManager.ActionsManager.AddAction(actionData);
            }

        }
    }
}