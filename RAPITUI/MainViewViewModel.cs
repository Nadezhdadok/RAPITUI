using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAPITUI
{
    internal class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand SelectCommand { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            SelectCommand = new DelegateCommand(OnSelectCommand);
            SelectCommand2 = new DelegateCommand(OnSelectCommand2);
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
        private void OnSelectCommand()
        {
            RaiseCloseRequest();

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Pipe> fInstances = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_PipeCurves)
                .WhereElementIsNotElementType()
                .Cast<Pipe>()
                .ToList();
            TaskDialog.Show("Количество труб", fInstances.Count.ToString());

            
        }





        //private ExternalCommandData2 _commandData2;

        public DelegateCommand SelectCommand2 { get; }

        //public MainViewViewModel(ExternalCommandData2 commandData2)
        //{
        //    _commandData2 = commandData2;
        //    SelectCommand2 = new DelegateCommand(OnSelectCommand2);
        //}

        //public event EventHandler CloseRequest2;
        //private void RaiseCloseRequest2()
        //{
        //    CloseRequest?.Invoke(this, EventArgs.Empty);
        //}
        private void OnSelectCommand2()
        {
            //RaiseCloseRequest();

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedRef = uidoc.Selection.PickObjects(ObjectType.Face);
            var volumeList = new List<double>();
            foreach (var selectedElement in selectedRef)
            {
                var element = doc.GetElement(selectedElement);


                if (element is Wall)
                {
                    Parameter volumeParameter = element.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                    if (volumeParameter.StorageType == StorageType.Double)
                    {
                        double volume = UnitUtils.ConvertFromInternalUnits(volumeParameter.AsDouble(), UnitTypeId.CubicMeters);
                        volumeList.Add(volume);
                    }

                }
            }
            TaskDialog.Show("Объем всех стен", $"{volumeList.Sum()} m3");


        }



       
        public DelegateCommand SelectCommand3 { get; }

       
        private void OnSelectCommand3()
        {
          

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            List<FamilyInstance> fInstances = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Doors)
                .WhereElementIsNotElementType()
                .Cast<FamilyInstance>()
                .ToList();
            TaskDialog.Show("Количество дверей", fInstances.Count.ToString());



        }
    }
}
