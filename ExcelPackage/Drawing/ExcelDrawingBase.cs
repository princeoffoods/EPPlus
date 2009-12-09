﻿/* 
 * You may amend and distribute as you like, but don't remove this header!
 * 
 * EPPlus provides server-side generation of Excel 2007 spreadsheets.
 * EPPlus is a fork of the ExcelPackage project
 * See http://www.codeplex.com/EPPlus for details.
 * 
 * All rights reserved.
 * 
 * EPPlus is an Open Source project provided under the 
 * GNU General Public License (GPL) as published by the 
 * Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
 * 
 * The GNU General Public License can be viewed at http://www.opensource.org/licenses/gpl-license.php
 * If you unfamiliar with this license or have questions about it, here is an http://www.gnu.org/licenses/gpl-faq.html
 * 
 * The code for this project may be used and redistributed by any means PROVIDING it is 
 * not sold for profit without the author's written consent, and providing that this notice 
 * and the author's name and all copyright notices remain intact.
 * 
 * All code and executables are provided "as is" with no warranty either express or implied. 
 * The author accepts no liability for any damage or loss of business that this product may cause.
 *
 * 
 * Code change notes:
 * 
 * Author							Change						Date
 * ******************************************************************************
 * Jan Källman		                Initial Release		        2009-10-01
 *******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
namespace OfficeOpenXml.Drawing
{
    public class ExcelDrawing : XmlHelper 
    {
        public class ExcelPosition : XmlHelper
        {
            XmlNode _node;
            XmlNamespaceManager _ns;            
            internal ExcelPosition(XmlNamespaceManager ns, XmlNode node) :
                base (ns,node)
            {
                _node = node;
                _ns = ns;
            }
            const string colPath="xdr:col";
            public int Column
            {
                get
                {
                    return GetXmlNodeInt(colPath);
                }
                set
                {
                    SetXmlNode(colPath, value.ToString());
                }
            }
            const string rowPath="xdr:row";
            public int Row
            {
                get
                {
                    return GetXmlNodeInt(rowPath);
                }
                set
                {
                    SetXmlNode(rowPath, value.ToString());
                }
            }
            const string colOffPath = "xdr:colOff";
            public int ColumnOff
            {
                get
                {
                    return GetXmlNodeInt(colOffPath);
                }
                set
                {
                    SetXmlNode(colOffPath, value.ToString());
                }
            }
            const string rowOffPath = "xdr:rowOff";
            public int RowOff
            {
                get
                {
                    return GetXmlNodeInt(rowOffPath);
                }
                set
                {
                    SetXmlNode(rowOffPath, value.ToString());
                }
            }
        }
        protected ExcelDrawings _drawings;
        protected XmlNode _topNode;
        string _nameXPath;
        internal ExcelDrawing(XmlNamespaceManager nameSpaceManager, XmlNode node, string nameXPath) :
            base(nameSpaceManager, node)
        {
            _nameXPath = nameXPath;
        }
        internal ExcelDrawing(ExcelDrawings drawings, XmlNode node, string nameXPath) :
            base(drawings.NameSpaceManager, node)
        {
            _drawings = drawings;
            _topNode = node;
            XmlNode posNode = node.SelectSingleNode("xdr:from", drawings.NameSpaceManager);
            if (node != null)
            {
                From = new ExcelPosition(drawings.NameSpaceManager, posNode);
            }
            posNode = node.SelectSingleNode("xdr:to", drawings.NameSpaceManager);
            if (node != null)
            {
                To = new ExcelPosition(drawings.NameSpaceManager, posNode);
            }
            _nameXPath = nameXPath;
        }
        public ExcelDrawing(XmlNamespaceManager nameSpaceManager, XmlNode node) :
            base(nameSpaceManager, node)
        {
        }
        public string Name 
        {
            get
            {
                try
                {
                    if (_nameXPath == "") return "";
                    return GetXmlNode(_nameXPath);
                }
                catch
                {
                    return ""; 
                }
            }
            set
            {
                try
                {
                    if (_nameXPath == "") throw new NotImplementedException();
                    SetXmlNode(_nameXPath, value);
                }
                catch
                {
                    throw new NotImplementedException();
                }
            }
        }
        public ExcelPosition From { get; set; }
        public ExcelPosition To { get; set; }
        /// <summary>
        /// Add new types Drawing types here
        /// </summary>
        /// <param name="drawings">The drawing collection</param>
        /// <param name="node">Xml top node</param>
        /// <returns>The Drawing object</returns>
        internal static ExcelDrawing GetDrawing(ExcelDrawings drawings, XmlNode node)
        {
            if (node.SelectSingleNode("xdr:sp", drawings.NameSpaceManager) != null)
            {
                return new ExcelShape(drawings, node);
            }
            else if (node.SelectSingleNode("xdr:pic", drawings.NameSpaceManager) != null)
            {
                return new ExcelPicture(drawings, node);
            }
            else if (node.SelectSingleNode("xdr:graphicFrame", drawings.NameSpaceManager) != null)
            {
                return new ExcelChart(drawings, node);
            }
            else
            {
                return new ExcelDrawing(drawings, node, "");
            }
        }


        internal string Id
        {
            get { return ""; }
        }
    }
}
