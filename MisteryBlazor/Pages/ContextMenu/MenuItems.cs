using Microsoft.AspNetCore.Components;
using MongoDB.Driver;
using Microsoft.AspNetCore.Components.Web;
namespace MisteryBlazor.Pages.ContextMenu
{
    public class MisteryMenuItem
    {
        public string ItemName { get; set; }
        public Action<MouseEventArgs> ItemAction { get; set; }

        public static MisteryMenuItem MisteryMenuDivider()
        {
            return new(Marcos.StringMarco.CONTEXT_MENU_DIVIDER);
        }
        public MisteryMenuItem(string name , Action<MouseEventArgs> action)
        {
            ItemName = name;
            ItemAction = action;
        }
        public MisteryMenuItem(string name)
        {
            ItemName = name;
            ItemAction = t => { return; };
        }
    }
}
