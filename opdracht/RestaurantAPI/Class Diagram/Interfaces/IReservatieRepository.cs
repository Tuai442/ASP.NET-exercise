using System;

namespace RestaurantBL.Interfaces
{
    public interface IReservatieRepository
    {
        void UpdateReservatie(Reservatie reservatie);
        void AnnuleerReservatie(ref int id);
        Reservatie GeefReservatie(int id);
        void VoegReservatieToe(Reservatie reservatie);
        bool HeeftReservatie(int reservatieNr);
        void VerwijderReservatie(Reservatie restaurant);
        List<Reservatie> GeefReservatieOpDatum(DateTime beginDatum, DateTime? eindDatum);
    }

}
