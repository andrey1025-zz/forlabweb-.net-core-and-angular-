using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IInstrument<Instrument, InstrumentList, getinstrument, ForecastIns, ForecastInsmodel, forecastinslist>
    {
      
        
               IEnumerable<InstrumentList> GetAll(int userid,string role);
            Instrument Getbyid(int id);
            int SaveData(getinstrument b);
            int UpdateData(int id, getinstrument b);
            int DeleteData(int id);
            Instrument GetInstrumentByName(string name);
        void saveforecastIns(int id,IEnumerable<ForecastIns> b);

        Instrument GetInstrumentByNameAndTestingArea(string name, int testingAreaId);
            IEnumerable<InstrumentList> GetListOfInstrumentByTestingArea(int testingAreaId);
            IEnumerable<Instrument> GetListOfInstrumentByTestingArea(string classofTest);
            List<InstrumentList> GetdefaultdataIns();
        void Updateforecastinstrument(ForecastInsmodel b);
        List<forecastinslist> getAllforecastinstrumentlist(int forecastid);

    }
}
