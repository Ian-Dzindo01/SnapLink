import { useState } from "react";

export default function ShortenForm() {
    const [originalUrl, setOriginalUrl] = useState("");
    const [shortenedUrl, setShortenedUrl] = useState(null);
    const [loading, setLoading] = useState(false);

    const handleShorten = async () => {
        if (!originalUrl.trim()) {
            alert("Please enter a valid URL.");
            return;
        }
        setLoading(true);
        try {
            const response = await fetch("http://localhost:5000/api/shortenurl", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ originalUrl }),
            });
            const data = await response.json();
            if (response.ok) {
                setShortenedUrl(data.shortenedUrl);
            } else {
                alert(data.message || "Failed to shorten URL");
            }
        } catch (error) {
            alert("Something went wrong. Please try again.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container">
            <h2>SnapLink - URL Shortener</h2>
            <input
                type="text"
                placeholder="Enter your URL here..."
                value={originalUrl}
                onChange={(e) => setOriginalUrl(e.target.value)}
            />
            <button onClick={handleShorten} disabled={loading}>
                {loading ? "Shortening..." : "Shorten URL"}
            </button>
            {shortenedUrl && (
                <div>
                    <p>Shortened URL:</p>
                    <a href={shortenedUrl} target="_blank" rel="noopener noreferrer">
                        {shortenedUrl}
                    </a>
                </div>
            )}
        </div>
    );
}
