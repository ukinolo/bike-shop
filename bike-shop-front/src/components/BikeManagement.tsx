import { useEffect, useState } from "react";
import Bike from "../model/Bike";
import { cities } from "../api/Cities";
import TableContainer from "@mui/material/TableContainer";
import Table from "@mui/material/Table";
import Paper from "@mui/material/Paper";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableCell from "@mui/material/TableCell";
import TableBody from "@mui/material/TableBody";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import InputLabel from "@mui/material/InputLabel";
import Select from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";
import { createNewBike, getBikes } from "../api/api";

export default function BikeManagement() {
    const [bikes, setBikes] = useState<Bike[]>([]);
    const [newBike, setNewBike] = useState(new Bike('', '', new Date(), '', false));
    const [selectedCity, setSelectedCity] = useState(cities.NoviSad);

    async function fetchBikes() {
        setBikes(await getBikes(selectedCity));
    }

    function createBike() {
        newBike.Rented = new Date();
        createNewBike(selectedCity, newBike);
        setNewBike(new Bike('', '', new Date(), '', false))
    }

    useEffect(() => {
        fetchBikes();
    }, [selectedCity])

    return(
    <>
        <InputLabel id="city-select-label">City</InputLabel>
        <Select
            labelId="city-select-label"
            value={selectedCity}
            onChange={(e) => {setSelectedCity(e.target.value)}}
        >
            <MenuItem value={cities.NoviSad}>Novi Sad</MenuItem>
            <MenuItem value={cities.Kragujevac}>Kragujevac</MenuItem>
            <MenuItem value={cities.Subotica}>Subotica</MenuItem>
        </Select>
        <TableContainer component={Paper} sx={{ height: 350, border: 0.5, marginTop: 2}}>
            <Table sx={{ minWidth: 400 }} aria-label="simple table">
                <TableHead>
                    <TableRow>
                        <TableCell>Id</TableCell>
                        <TableCell align="right">Type</TableCell>
                        <TableCell align="right">Rented</TableCell>
                        <TableCell align="right">CustomerId</TableCell>
                        <TableCell align="right">Returned</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {bikes.map((bike) => (
                        <TableRow
                        key={bike.Id}
                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                        >
                        <TableCell component="th" scope="row">
                            {bike.Id}
                        </TableCell>
                        <TableCell align="right">{bike.Type}</TableCell>
                        <TableCell align="right">{bike.Rented.toUTCString()}</TableCell>
                        <TableCell align="right">{bike.CustomerId}</TableCell>
                        <TableCell align="right">{bike.Returned ? 'true' : 'false'}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
        <br/>
        <form>
            <TextField label="Id" variant="outlined" fullWidth margin="dense" value={newBike.Id} onChange={e => setNewBike({...newBike, Id: e.target.value})}/>
            <TextField label="Type" variant="outlined" fullWidth margin="dense" value={newBike.Type} onChange={e => setNewBike({...newBike, Type: e.target.value})}/>
            <TextField label="CustomerId" variant="outlined" fullWidth margin="dense" value={newBike.CustomerId} onChange={e => setNewBike({...newBike, CustomerId: e.target.value})}/>
            <Button variant="contained" color="primary" onClick={() => createBike()}>Create new Bike</Button>
        </form>
    </>
    );
}