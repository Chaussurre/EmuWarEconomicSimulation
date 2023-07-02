using System.Collections;
using UnityEngine;

namespace Cardinator.Standard
{
    public class Player
    {
        public int playerID;
        public PlayerManager PlayerManager;

        public CardStack Hand;
        public CardStack Units;

        public Player(int ID, PlayerManager playerManager)
        {
            playerID = ID;
            PlayerManager = playerManager;
        }

        public static implicit operator int(Player player) => player.playerID;

        public bool PlayCard(int CardID) => PlayerManager.PlayCard(this, CardID);

        public bool Attack(int AttackerID, int TargetID) => PlayerManager.AttackCard(this, AttackerID, TargetID);
    }
}