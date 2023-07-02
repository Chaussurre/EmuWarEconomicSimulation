using UnityEngine;
            
namespace Cardinator.Standard
{
    public class ActionDrawWatcher : ActionWatcher<CardData, ActionDrawWatcher.DrawData>
    {
        public PlayField PlayerFields;

        protected override void Apply(DrawData actionData)
        {
            for (int i = 0; i < actionData.cards; i++)
                DrawOne(actionData.player);
        }

        private void DrawOne(Player player)
        {
            var card = new CardStack.CardPosition(PlayerFields.Fields[player].Deck).GetCard();

            if (!card.HasValue)
                return;

            CardManager.MoveCard(card.Value.CardID, new(PlayerFields.Fields[player].Hand));
            CardManager.ChangeVisibility(card.Value.CardID, card.Value.VisibleMask + player);
        }

        public static DrawData Draw(Player player, int cards)
        {
            return new() { player = player, cards = cards };
        }


        public struct DrawData
        {
            public Player player;
            public int cards;
        }

    }
}