/* eslint-disable no-unused-vars */
import { useState } from "react";
import axios from "axios";
import { Button, TextField as Input, Card } from "@mui/material";

export default function ShortenForm() {
    const [url, setUrl] = useState("");
    const [shortenedUrl, setShortenedUrl] = useState(null);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);

    const handleShorten = async () => {
        try {
            setError(null);
            setLoading(true);
            console.log("Sending request to shorten URL:", url);
            const response = await axios.post("/api/ShortenUrl", { originalUrl: url });
            console.log("Response received:", response.data);
            setShortenedUrl(response.data.ShortenedUrl);
        } catch (err) {
            console.error("Error while shortening URL:", err);
            setError("Failed to shorten URL. Please try again.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <Card className="p-4 max-w-md mx-auto mt-10">
            <h2 className="text-xl font-bold mb-4">URL Shortener</h2>
            <Input
                type="url"
                placeholder="Enter a URL"
                value={url}
                onChange={(e) => setUrl(e.target.value)}
                className="mb-2"
            />
            <Button onClick={handleShorten} className="w-full" disabled={loading}>
                {loading ? "Shortening..." : "Shorten"}
            </Button>
            {loading && <p>Loading...</p>}
            {shortenedUrl && (
                <p className="mt-4">
                    Shortened URL: <a href={shortenedUrl} className="text-blue-500" target="_blank" rel="noopener noreferrer">{shortenedUrl}</a>
                </p>
            )}
            {error && <p className="text-red-500 mt-2">{error}</p>}
        </Card>
    );
}
