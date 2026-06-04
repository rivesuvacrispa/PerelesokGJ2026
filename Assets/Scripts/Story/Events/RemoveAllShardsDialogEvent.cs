using Interaction.Objects;

namespace Story.Events
{
    public class RemoveAllShardsDialogEvent : DialogEvent
    {
        public override void Fire()
        {
            MirrorShard.ClearAll();
        }
    }
}