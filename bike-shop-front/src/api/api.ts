import axios from "axios";
import Bike from "../model/Bike";
import { cities, type Cities } from "./Cities";
import Customer from "../model/Customer";

const central_api = "http://localhost:8080";
const office_api = {ns: "http://localhost:8081", kg: "http://localhost:8082", su:"http://localhost:8083"};


function getBaseApi(city: Cities) {
    switch (city) {
        case cities.NoviSad:
            return office_api.ns;
        case cities.Kragujevac:
            return office_api.kg;
        case cities.Subotica:
            return office_api.su;
    }
}

export async function getBikes(city: Cities): Promise<Bike[]> {
    try {
        let response = await axios.get(getBaseApi(city) + '/bike');
        let ret: Bike[] = [];
        response.data.forEach((element: { id: string; type: string; rented: string, customerId: string; returned: boolean}) => {
            ret.push(new Bike(element.id, element.type, new Date(element.rented), element.customerId, element.returned));
        });
        return ret;
    }
    catch(error) {
        alert(error);
        return [];
    }
}

export function createNewBike(city: Cities, bike: Bike) {
    axios.post(getBaseApi(city) + '/bike', bike).then(response => {
        if (response.status == 200) {
            alert("Bike succesfuly created");
        } else {
            alert("Error creating a bike, got following error:" + response.data);
        }
    }).catch(error => {
        alert(error);
    })
}

export function returnBike(city: Cities, bikeId: number) {
    axios.put(getBaseApi(city) + '/bike/' + bikeId.toString() + '/return').then(response => {
        if (response.status == 200) {
            alert("Bike succesfuly returned");
        } else {
            alert("Error returning a bike, got following error:" + response.data);
        }
    }).catch(error => {
        alert(error);
    })
}

export async function getCustomers(): Promise<Customer[]> {
    try {
        let response = await axios.get(central_api + '/customer');
        let ret: Customer[] = [];
        response.data.forEach((element: { id: string; name: string; surname: string; address: string; rentedBikes: number}) => {
            ret.push(new Customer(element.id, element.name, element.surname, element.address, element.rentedBikes));
        });
        return ret;
    }
    catch(error) {
        alert(error);
        return [];
    }
}

export function createNewCustomer(customer: Customer) {
    axios.post(central_api + '/customer', customer).then(response => {
        alert(response.statusText);
    }).catch(error => {
        alert(error);
    })
}