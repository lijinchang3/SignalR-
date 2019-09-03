using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace MvcApplication1.Hubs
{
    public class WorkflowHub : Hub
    {
        /// <summary>
        /// 静态用户列表
        /// </summary>
        private IList<string> userList = UserInfo.userList;

        /// <summary>
        /// 用户的connectionID与用户名对照表
        /// </summary>
        private readonly static Dictionary<string, string> _connections = new Dictionary<string, string>();


       /// <summary>
       /// 发送函数，前端触发该函数给服务器，服务器在将消息发送给前端，（Clients.All.(函数名)是全体广播，另外Clients提供了组播，广播排除，组播排除，指定用户播发等等）
       /// 该函数名在前端使用时一定要注意，前端调用该函数时，函数首字母一定要小写
       /// </summary>
       /// <param name="name1">发起者</param>
       /// <param name="name2">消息接收者</param>
        public void SendByGroup(string name1, string name2)
        {
            //_messenger.BroadCastMessage(message, group);
            //Client内为用户的id，是唯一的，SendMessage函数是前端函数，意思是服务器将该消息推送至前端
            Clients.Client(_connections[name2]).SendMessage("来自用户"+name1 + " " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")+"的消息推送！");
        }

        /// <summary>
        /// 用户上线函数
        /// </summary>
        /// <param name="name"></param>
        public void SendLogin(string name)
        {
            if (!userList.Contains(name))
            {
                userList.Add(name);
                //这里便是将用户id和姓名联系起来
                _connections.Add(name, Context.ConnectionId);  
            }
            else
            {
                //每次登陆id会发生变化
                _connections[name] = Context.ConnectionId;
            }
            //新用户上线，服务器广播该用户名
            //this.Groups.Add(Context.ConnectionId, name);
            Clients.All.loginUser(userList);
          
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }

}