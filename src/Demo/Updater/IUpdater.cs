using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Updater
{
    public interface IUpdater
    {
        /// <summary>
        /// 运行自动升级程序
        /// </summary>
        bool Run();
    }
}
