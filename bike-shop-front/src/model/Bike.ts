class Bike{
    Id: string;
    Type: string;
    Rented: Date;
    CustomerId: string;
    Returned: boolean;

    constructor(id: string, type: string, rented: Date, customerId: string, returned: boolean) {
        this.Id = id;
        this.Type = type;
        this.Rented = rented;
        this.CustomerId = customerId;
        this.Returned = returned;
    }
}

export default Bike;