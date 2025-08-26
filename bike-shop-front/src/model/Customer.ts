class Customer{
    Id: string;
    Name: string;
    Surname: string;
    Address: string;
    RentedBikes: number;

    constructor(id: string, name: string, surname: string, address: string, rentedBikes: number) {
        this.Id = id;
        this.Name = name;
        this.Surname = surname;
        this.Address = address;
        this.RentedBikes = rentedBikes;
    }
}

export default Customer;