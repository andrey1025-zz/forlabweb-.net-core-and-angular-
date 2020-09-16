using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
 
    public class MMProgram
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string ProgramName { get; set; }
        public string Description { get; set; }

        public int Gtp { get; set; }
        public int Rtp { get; set; }
        public int NoofYear { get; set; }
        public int? UserId { get; set; }
        public IList<MMForecastParameterList> _mMForecastParameter { get; set; }
       // public IList<MMGeneralAssumptionList> _mMGeneralAssumption { get; set; }
    }
    public class MMProgramList
    {
        public int Id { get; set; }
        public string ProgramName { get; set; }
        public string Forecastmethod { get; set; }

        public int TotalGrp { get; set; }
        public int Totalforecast { get; set; }
    }

    public class MMForecastParameter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int ForecastMethod { get; set; }
        public string VariableName { get; set; }
        public int VariableDataType { get; set; }
        public string UseOn { get; set; }
        public string VariableFormula { get; set; }
        public int ProgramId { get; set; }
        public string VarCode { get; set; }
        public bool IsPrimaryOutput { get; set; }
        public bool VariableEffect { get; set; }
        public bool IsActive { get; set; }
        public int Entity_type_id { get; set; }
        public int? UserId { get; set; }

        public int Forecastid { get; set; }

    }



    public class MMGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
       
        public string GroupName { get; set; }
        public int ProgramId { get; set; }
        public int? UserId { get; set; }

        public bool IsActive { get; set; }
   
    }
    public class DemographicMMGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string GroupName { get; set; }
        public int ProgramId { get; set; }
        public int? UserId { get; set; }

        public bool IsActive { get; set; }

        public int Forecastid { get; set; }



    }
    public class DemographicMMGroupList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string GroupName { get; set; }
        public int ProgramId { get; set; }

        public int forecastid { get; set; }
        public bool IsActive { get; set; }
        public string type { get; set; }
    }
    public class Suggustionlist
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
        public string Name { get; set; }


        public string Type_name { get; set; }


        public int? UserId { get; set; }


    }
    public class MMGeneralAssumption
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
     
        public string VariableName { get; set; }
        public int VariableDataType { get; set; }
        public string UseOn { get; set; }
        public string VariableFormula { get; set; }
        public int ProgramId { get; set; }
        public string VarCode { get; set; }
        public int AssumptionType { get; set; }
        public bool VariableEffect { get; set; }
        public bool IsActive { get; set; }
        public int Entity_type_id { get; set; }
        public int? UserId { get; set; }
       

    }
    public class demographicMMGeneralAssumption
    {
        public int Id { get; set; }

        public string VariableName { get; set; }
        public int VariableDataType { get; set; }
        public string UseOn { get; set; }
        public string VariableFormula { get; set; }
        public int ProgramId { get; set; }
        public string VarCode { get; set; }
        public int AssumptionType { get; set; }
        public bool VariableEffect { get; set; }
        public bool IsActive { get; set; }
        public int Entity_type_id { get; set; }
        public int? UserId { get; set; }
        public int Forecastid { get; set; }
    }
    public class MMGeneralAssumptionList
    {
        public int Id { get; set; }

        public string VariableName { get; set; }
        public int VariableDataType { get; set; }
        public string VariableDataTypeName { get; set; }
        public string UseOn { get; set; }
        public string VariableFormula { get; set; }
        public int ProgramId { get; set; }
        public string VarCode { get; set; }

        public int AssumptionType { get; set; }
        public string AssumptionTypename { get; set; }
        public bool VariableEffect { get; set; }
        public string IsActive { get; set; }
    }
        public class MMGroupList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string GroupName { get; set; }
        public int ProgramId { get; set; }

        public int forecastid { get; set; }
        public string IsActive { get; set; }
    }
    public class MMForecastParameterList
    {
      
        public int Id { get; set; }
        public int ForecastMethod { get; set; }
        public string ForecastMethodname { get; set; }

        public string VariableName { get; set; }
        public int VariableDataType { get; set; }
        public string VariableDataTypename { get; set; }
        public string UseOn { get; set; }
      
        public int ProgramId { get; set; }
        public string VarCode { get; set; }
        public bool IsPrimaryOutput { get; set; }
        public bool VariableEffect { get; set; }
        public string IsActive { get; set; }
        public int forecastid { get; set; }
    }

    public class entity_type
    {
        public int id { get; set; }
        public string Type { get; set; }
        public int parameter_type_id { get; set; }
    }
    public class entity_parameter_type
    {
        public int id { get; set; }
        public string Name { get; set; }
    }
   
}
