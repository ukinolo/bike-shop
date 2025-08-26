import { useEffect, useState } from "react";
import Customer from "../model/Customer";
import TableContainer from "@mui/material/TableContainer";
import Table from "@mui/material/Table";
import Paper from "@mui/material/Paper";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import { Button, TableBody, TableCell, TextField } from "@mui/material";
import { createNewCustomer, getCustomers } from "../api/api";


export default function CustomerManagement() {
    const [customers, setCustomers] = useState<Customer[]>([]);
    const [newCustomer, setNewCustomer] = useState(new Customer('', '', '', '', 0));
    
    async function fetchCustomers() {
        setCustomers(await getCustomers());
    }
    
    function createCustomer(): void {
        createNewCustomer(newCustomer);
        setNewCustomer(new Customer('', '', '', '', 0));
    }

    useEffect(() => {
        fetchCustomers();
    }, []);
    

    return (
        <>
            <Button onClick={() => fetchCustomers()} variant="contained" color="primary">Refresh table</Button>
            <TableContainer component={Paper} sx={{ height: 350, border: 0.5, marginTop: 2}}>
                <Table sx={{ minWidth: 400 }} aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Id</TableCell>
                            <TableCell align="right">Name</TableCell>
                            <TableCell align="right">Surname</TableCell>
                            <TableCell align="right">Address</TableCell>
                            <TableCell align="right">Rented bikes</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {customers.map((customer) => (
                            <TableRow
                            key={customer.Id}
                            sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                            >
                            <TableCell component="th" scope="row">
                                {customer.Id}
                            </TableCell>
                            <TableCell align="right">{customer.Name}</TableCell>
                            <TableCell align="right">{customer.Surname}</TableCell>
                            <TableCell align="right">{customer.Address}</TableCell>
                            <TableCell align="right">{customer.RentedBikes}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <br/>
            <form>
                <TextField label="Id" variant="outlined" fullWidth margin="dense" value={newCustomer.Id} onChange={e => setNewCustomer({...newCustomer, Id: e.target.value})}/>
                <TextField label="Name" variant="outlined" fullWidth margin="dense" value={newCustomer.Name} onChange={e => setNewCustomer({...newCustomer, Name: e.target.value})}/>
                <TextField label="Surname" variant="outlined" fullWidth margin="dense" value={newCustomer.Surname} onChange={e => setNewCustomer({...newCustomer, Surname: e.target.value})}/>
                <TextField label="Address" variant="outlined" fullWidth margin="dense" value={newCustomer.Address} onChange={e => setNewCustomer({...newCustomer, Address: e.target.value})}/>
                <Button variant="contained" color="primary" onClick={() => createCustomer()}>Create new Customer</Button>
            </form>
        </>
    );

}