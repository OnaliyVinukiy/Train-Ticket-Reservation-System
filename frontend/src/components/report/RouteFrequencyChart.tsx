import { BarChart, Bar, XAxis, YAxis, Tooltip, CartesianGrid, ResponsiveContainer } from "recharts";
import { ChartBarIcon } from "@heroicons/react/24/outline";

interface Props {
    data: {
        route: string;
        count: number;
    }[];
}

function RouteFrequencyChart({ data }: Props) {
    const CustomTooltip = ({ active, payload }: any) => {
        if (active && payload && payload.length) {
            return (
                <div className="bg-white/90 backdrop-blur-sm rounded-xl shadow-lg border border-gray-100 px-4 py-2 text-sm">
                    <p className="font-semibold text-gray-800">{payload[0].payload.route}</p>
                    <p className="text-indigo-600">
                        Bookings: <span className="font-bold">{payload[0].value}</span>
                    </p>
                </div>
            );
        }
        return null;
    };

    return (
        <div className="bg-white rounded-2xl shadow-lg p-6 md:p-8">
            <div className="flex items-center justify-between mb-6">
                <div className="flex items-center gap-2">
                    <ChartBarIcon className="w-6 h-6 text-indigo-600" />
                    <h2 className="text-2xl font-bold text-gray-800">Popular Routes</h2>
                </div>
                <span className="text-sm text-gray-400 bg-gray-50 px-3 py-1 rounded-full">
                    Last 7 days
                </span>
            </div>
            <ResponsiveContainer width="100%" height={300}>
                <BarChart data={data} margin={{ top: 10, right: 10, left: 0, bottom: 5 }}>
                    <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" vertical={false} />
                    <XAxis
                        dataKey="route"
                        tick={{ fill: "#6b7280", fontSize: 12 }}
                        axisLine={{ stroke: "#e5e7eb" }}
                        tickLine={false}
                    />
                    <YAxis
                        tick={{ fill: "#6b7280", fontSize: 12 }}
                        axisLine={false}
                        tickLine={false}
                        allowDecimals={false}
                    />
                    <Tooltip content={<CustomTooltip />} cursor={{ fill: "rgba(99,102,241,0.05)" }} />
                    <Bar
                        dataKey="count"
                        fill="url(#colorGradient)"
                        radius={[6, 6, 0, 0]}
                        barSize={36}
                    />
                    <defs>
                        <linearGradient id="colorGradient" x1="0" y1="0" x2="0" y2="1">
                            <stop offset="0%" stopColor="#818cf8" />
                            <stop offset="100%" stopColor="#6366f1" />
                        </linearGradient>
                    </defs>
                </BarChart>
            </ResponsiveContainer>
        </div>
    );
}

export default RouteFrequencyChart;