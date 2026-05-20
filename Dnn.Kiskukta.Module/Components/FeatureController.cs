/*
' Copyright (c) 2026 Kiskukta
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System.Collections.Generic;
//using System.Xml;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace Dnn.Kiskukta.Dnn.Kiskukta.Module.Components
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for Dnn.Kiskukta.Module
    /// 
    /// The FeatureController class is defined as the BusinessController in the manifest file (.dnn)
    /// DotNetNuke will poll this class to find out which Interfaces the class implements. 
    /// 
    /// The IPortable interface is used to import/export content from a DNN module
    /// 
    /// The ISearchable interface is used by DNN to index the content of a module
    /// 
    /// The IUpgradeable interface allows module developers to execute code during the upgrade 
    /// process for a module.
    /// 
    /// Below you will find stubbed out implementations of each, uncomment and populate with your own data
    /// </summary>
    /// -----------------------------------------------------------------------------

    //uncomment the interfaces to add the support.
    public class FeatureController //: IPortable, ISearchable, IUpgradeable
    {


        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        //public string ExportModule(int ModuleID)
        //{
        //string strXML = "";

        //List<Dnn.Kiskukta.ModuleInfo> colDnn.Kiskukta.Modules = GetDnn.Kiskukta.Modules(ModuleID);
        //if (colDnn.Kiskukta.Modules.Count != 0)
        //{
        //    strXML += "<Dnn.Kiskukta.Modules>";

        //    foreach (Dnn.Kiskukta.ModuleInfo objDnn.Kiskukta.Module in colDnn.Kiskukta.Modules)
        //    {
        //        strXML += "<Dnn.Kiskukta.Module>";
        //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objDnn.Kiskukta.Module.Content) + "</content>";
        //        strXML += "</Dnn.Kiskukta.Module>";
        //    }
        //    strXML += "</Dnn.Kiskukta.Modules>";
        //}

        //return strXML;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        //public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        //{
        //XmlNode xmlDnn.Kiskukta.Modules = DotNetNuke.Common.Globals.GetContent(Content, "Dnn.Kiskukta.Modules");
        //foreach (XmlNode xmlDnn.Kiskukta.Module in xmlDnn.Kiskukta.Modules.SelectNodes("Dnn.Kiskukta.Module"))
        //{
        //    Dnn.Kiskukta.ModuleInfo objDnn.Kiskukta.Module = new Dnn.Kiskukta.ModuleInfo();
        //    objDnn.Kiskukta.Module.ModuleId = ModuleID;
        //    objDnn.Kiskukta.Module.Content = xmlDnn.Kiskukta.Module.SelectSingleNode("content").InnerText;
        //    objDnn.Kiskukta.Module.CreatedByUser = UserID;
        //    AddDnn.Kiskukta.Module(objDnn.Kiskukta.Module);
        //}

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// -----------------------------------------------------------------------------
        //public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        //{
        //SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

        //List<Dnn.Kiskukta.ModuleInfo> colDnn.Kiskukta.Modules = GetDnn.Kiskukta.Modules(ModInfo.ModuleID);

        //foreach (Dnn.Kiskukta.ModuleInfo objDnn.Kiskukta.Module in colDnn.Kiskukta.Modules)
        //{
        //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objDnn.Kiskukta.Module.Content, objDnn.Kiskukta.Module.CreatedByUser, objDnn.Kiskukta.Module.CreatedDate, ModInfo.ModuleID, objDnn.Kiskukta.Module.ItemId.ToString(), objDnn.Kiskukta.Module.Content, "ItemId=" + objDnn.Kiskukta.Module.ItemId.ToString());
        //    SearchItemCollection.Add(SearchItem);
        //}

        //return SearchItemCollection;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="Version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        //public string UpgradeModule(string Version)
        //{
        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        #endregion

    }

}
