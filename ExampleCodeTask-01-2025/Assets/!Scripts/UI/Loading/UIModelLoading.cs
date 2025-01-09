using UI.Views;

namespace UI.Models
{
    public class UIModelLoading : UIModel<UIViewLoading>
    {
        public override bool IsSilent => true;
        public override string ViewName => "UI/Loading";
    }
}
