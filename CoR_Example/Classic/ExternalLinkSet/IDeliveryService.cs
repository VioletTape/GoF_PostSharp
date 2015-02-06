namespace CoR_Example.Classic.ExternalLinkSet {
    public abstract class Handler
    {
        protected readonly Handler[] Handlers;

        protected Handler(params Handler[] handlers)
        {
            Handlers = handlers;
        }

        public abstract void CalcDelivery(Address address);
    }

    public class NullHandler : Handler
    {
        public NullHandler() : base(null) { }

        public override void CalcDelivery(Address address) { }
    }

    public class LocalDeliveryService : Handler
    {
        public LocalDeliveryService(params Handler[] handlers) : base(handlers) { }

        public override void CalcDelivery(Address address)
        {
            if (address.Country == "USA"
                && address.State == "Utah")
            {
                address.SetDeliveryPrice(0);
                return;
            }

            foreach (var handler in Handlers)
            {
                handler.CalcDelivery(address);
            }
        }
    }


    public abstract class UsaLocalDelivery : Handler
    {
        protected UsaLocalDelivery(params Handler[] handlers) : base(handlers) { }

        public override void CalcDelivery(Address address)
        {
            if (address.Country == "USA")
            {
                CalcUsaDelivery(address);
                return;
            }

            foreach (var handler in Handlers)
            {
                handler.CalcDelivery(address);
            }
        }

        public abstract void CalcUsaDelivery(Address address);
    }

    public class CountryManDelivery : UsaLocalDelivery
    {

        public CountryManDelivery(params Handler[] handlers) : base(handlers) { }
        public override void CalcUsaDelivery(Address address)
        {
            address.SetDeliveryPrice(0);
        }
    }

    public class AllUsaDelivery : UsaLocalDelivery
    {

        public AllUsaDelivery(params Handler[] handlers) : base(handlers) { }
        public override void CalcUsaDelivery(Address address)
        {
            address.SetDeliveryPrice(0);
        }
    }

    public class LittlePonyExpressUsa : UsaLocalDelivery
    {

        public LittlePonyExpressUsa(params Handler[] handlers) : base(handlers) { }
        public override void CalcUsaDelivery(Address address)
        {
            address.SetDeliveryPrice(0);
        }
    }

    public abstract class WorldwideDelivery : Handler
    {
        protected WorldwideDelivery(params Handler[] handlers) : base(handlers) { }

        public override void CalcDelivery(Address address)
        {
            if (address.Country == "USA")
            {
                CalcWorldwideDelivery(address);
                return;
            }

            foreach (var handler in Handlers)
            {
                handler.CalcDelivery(address);
            }
        }

        public abstract void CalcWorldwideDelivery(Address address);
    }

    public class LittlePonyExpress : WorldwideDelivery
    {

        public LittlePonyExpress(params Handler[] handlers) : base(handlers) { }
        public override void CalcWorldwideDelivery(Address address)
        {
            address.SetDeliveryPrice(0);
        }
    }

    public class WorldExpress : WorldwideDelivery
    {

        public WorldExpress(params Handler[] handlers) : base(handlers) { }
        public override void CalcWorldwideDelivery(Address address)
        {
            address.SetDeliveryPrice(0);
        }
    }

   
}