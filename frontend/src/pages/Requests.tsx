import { useEffect, useState } from "react";
import type { SpecialRequest } from "../types/specialRequest";
import { getRequests, deleteRequest, updateRequest } from "../services/requestService";

function Requests() {
    const [requests, setRequests] = useState<SpecialRequest[]>([]);

    useEffect(() => {
        loadRequests();
    }, []);

    const loadRequests = async () => {
        const data = await getRequests();
        setRequests(data);
    };

    const handleDelete = async (id: number) => {
        await deleteRequest(id);
        loadRequests();
    };

    const handleUpdate = async (request: SpecialRequest) => {
        await updateRequest(request.id, request);
        loadRequests();
    };

    return (
        <div className="p-8">
            <h1 className="text-3xl font-bold mb-6">Special Requests</h1>
            <div className="grid gap-4">
                {requests.map(request => (
                    <div
                        key={request.id}
                        className="bg-white shadow rounded-xl p-5 flex gap-4 items-center"
                    >
                        <input
                            className="border rounded p-2 flex-1"
                            value={request.description}
                            onChange={(e) =>
                                setRequests(
                                    requests.map(item =>
                                        item.id === request.id
                                            ? { ...item, description: e.target.value }
                                            : item
                                    )
                                )
                            }
                        />
                        <button
                            onClick={() => handleUpdate(request)}
                            className="bg-blue-600 text-white px-4 py-2 rounded-lg"
                        >
                            Update
                        </button>
                        <button
                            onClick={() => handleDelete(request.id)}
                            className="bg-red-600 text-white px-4 py-2 rounded-lg"
                        >
                            Delete
                        </button>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default Requests;