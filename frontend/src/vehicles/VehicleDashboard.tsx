import React, { useState, useEffect } from 'react'
import { IVehicle } from './IVehicle';

const VehicleDashboard = () => {
    const [vehicles, setVehicles] = useState<IVehicle[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string|null>(null);

    useEffect(() => {
        fetch('http://localhost:5017/Vehicles')
            .then(response => {
                if (!response.ok){
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                setVehicles(data);
                setLoading(false);
            })
            .catch(err => {
                setError(err.message);
                setLoading(false);
            });
    }, []);

    if (loading) {
        return <div>Loading...</div>;
    }
    
    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div>
            <h2>Vehicle Dashboard</h2>
            <table>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>License Plate</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {vehicles.map(vehicle => (
                        <tr key={vehicle.id}>
                            <td>{vehicle.id}</td>
                            <td>{vehicle.licensePlate}</td>
                            <td>{vehicle.status}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default VehicleDashboard;