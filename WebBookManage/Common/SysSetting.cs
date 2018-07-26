using System;
using System.Collections.Generic;
using System.Text;
using WebBookManage.Model;

namespace WebBookManage.Common
{
    public class SysSetting
    {
        private static SysSetting Instance = null;

        private ThemeModel _themeModel;

        public static SysSetting GetInstance() {
            if (Instance == null)
                Instance = new SysSetting();
            return Instance;
        }
        
        public ThemeModel ThemeModel
        {
            set { _themeModel = value; }
            get
            {
                if (_themeModel == null)
                {
                    _themeModel = new ThemeModel("素白");
                }
                return _themeModel;
            }
        }


    }
}
