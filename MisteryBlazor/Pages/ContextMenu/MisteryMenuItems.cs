using Microsoft.AspNetCore.Components;
using MongoDB.Driver;
using Microsoft.AspNetCore.Components.Web;
namespace MisteryBlazor.Pages.ContextMenu
{
    /// <summary>
    /// 定义单个右键上下文菜单的值和操作 <br/>
    /// 本类与 MisteryContextMenu 组件息息相关
    /// </summary>
    public class MisteryMenuItem
    {
        public string ItemName { get; set; }
        public Action<MouseEventArgs> ItemAction { get; set; }

        /// <summary>
        /// 快速取得一个定义分隔符的 MisteryMenuItem
        /// </summary>
        /// <returns>用 CONTEXT_MENU_DIVIDER 宏定义的 MisteryMenuItem</returns>
        public static MisteryMenuItem Divider()
        {
            return new(Marcos.StringMarco.CONTEXT_MENU_DIVIDER);
        }
        /// <summary>
        /// 初始化该 MisteryMenuItem 的显示值和点击时操作
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public MisteryMenuItem(string name , Action<MouseEventArgs> action)
        {
            ItemName = name;
            ItemAction = action;
        }
        /// <summary>
        /// 初始化该 MisteryMenuItem 的显示值
        /// </summary>
        /// <param name="name"></param>
        public MisteryMenuItem(string name)
        {
            ItemName = name;
            ItemAction = t => { return; };
        }
    }
}
