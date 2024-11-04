using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Entities.Enums
{
    
    public enum ProductConditions
    {
        New, 
        Used
    }
    public enum State{
        Enable,
        Disabled

    }
    public enum ProductState
    {
        Pending,  // pendiente
        Active, // habilitado o activo
        Disabled, // deshabilitado 
        Sold // vendido
    }
    public enum AuctionState{
        Pending, // pendiente o proxima
        Active, 
        Closed,  // cerrada (llega a su fecha de cierre)
        Canceled, // cancelada
        NoProducts
    }
    public enum PaymentMethods
    {
        Efectivo, //= 0 "efectivo"
        Cheque,
        TreanferenciaBancaria,
        TarjetaDeCredito,
        TarjetaDeDebito
    }
    public enum DocumType
    {
        DNI, //0
        CUIT //1

    }
    public enum DeliveryModes
    {
        Domicilio,
        RetiroLocal,
        Internacional
    }
    public enum PersonType
    {
        Fisica,
        Juridica
    }


}
