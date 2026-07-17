import { useEffect, useState } from "react";
import type { SpecialRequest } from "../types/specialRequest";

import {
    getRequests,
    createRequest,
    deleteRequest
} from "../services/requestService";


function Requests() {

    const [requests, setRequests] = useState<SpecialRequest[]>([]);

    const [newRequest, setNewRequest] = useState<SpecialRequest>({
        id: 0,
        description: ""
    });

    useEffect(() => {
        loadRequests();
    }, []);

    const loadRequests = async () => {
        const data = await getRequests();

        setRequests(data);
    };

    const handleCreate = async () => {
        await createRequest(newRequest);

        setNewRequest({
            id: 0,
            description: ""
        });

        loadRequests();
    };

    const handleDelete = async (id: number) => {
        await deleteRequest(id);

        loadRequests();
    };

    return (
        <div className="p-8">

            <h1 className="text-3xl font-bold mb-6">
                Special Requests
            </h1>

            <div className="
                bg-white
                rounded-xl
                shadow
                p-6
                mb-8
            ">
                <h2 className="text-xl font-semibold mb-4">
                    Add Request
                </h2>

                <input
                    className="
                        border
                        rounded
                        p-2
                        w-full
                        mb-4
                    "
                    placeholder="Example: Window seat"
                    value={newRequest.description}
                    onChange={(e) =>
                        setNewRequest({
                            ...newRequest,
                            description: e.target.value
                        })
                    }
                />

                <button
                    onClick={handleCreate}
                    className="
                        bg-blue-600
                        text-white
                        px-4
                        py-2
                        rounded-lg
                    "
                >
                    Add Request
                </button>
            </div>

            <div className="grid gap-4">

                {requests.map((request) => (

                    <div
                        key={request.id}
                        className="
                            bg-white
                            shadow
                            rounded-xl
                            p-5
                            flex
                            justify-between
                            items-center
                        "
                    >

                        <p>
                            {request.description}
                        </p>

                        <button
                            onClick={() => handleDelete(request.id)}
                            className="
                                bg-red-500
                                text-white
                                px-4
                                py-2
                                rounded-lg
                            "
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