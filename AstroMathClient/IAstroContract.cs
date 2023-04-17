using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroMathClient
{
    [ServiceContract]
    internal interface IAstroContract
    {
        [OperationContract]
        double StarVelocity(double oWL, double rWL);

        [OperationContract]
        double StarDistance(double angle);

        [OperationContract]
        double Kelvin(double celcius);

        [OperationContract]
        double EventHorizon(double mass);
    }
}
