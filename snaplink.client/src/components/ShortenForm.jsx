import { useState } from "react";

export default function HealthCheck() {
    const [status, setStatus] = useState(null);

    const checkBackend = async () => {
        try {
            const response = await fetch("http://localhost:5000/api/ShortenUrl/ping");
            const data = await response.text();
            if (response.ok) {
                setStatus(`Backend is running: ${data}`);
            } else {
                setStatus("Backend is not responding");
            }
        } catch (error) {
            setStatus("Error: Could not reach backend");
        }
    };

    return (
        <div>
            <button onClick={checkBackend}>Ping Backend</button>
            {status && <p>{status}</p>}
        </div>
    );
}
