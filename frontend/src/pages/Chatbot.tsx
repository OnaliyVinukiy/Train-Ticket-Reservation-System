import { useState, useRef, useEffect } from "react";
import { sendMessage } from "../services/chatbotService";
import { UserIcon, SparklesIcon, ChatBubbleLeftRightIcon } from "@heroicons/react/24/solid";

interface Message {
    sender: "user" | "bot";
    text: string;
}

function Chatbot() {
    const [messages, setMessages] = useState<Message[]>([
        {
            sender: "bot",
            text: "Hello! I can help predict train availability and ticket price trends. Ask me about a route."
        }
    ]);
    const [input, setInput] = useState("");
    const [loading, setLoading] = useState(false);
    const messagesEndRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    }, [messages, loading]);

    const handleSend = async () => {
        if (!input.trim() || loading) return;

        const userMessage: Message = { sender: "user", text: input };
        setMessages(prev => [...prev, userMessage]);
        setLoading(true);

        try {
            const response = await sendMessage(input);
            const botMessage: Message = { sender: "bot", text: response.reply };
            setMessages(prev => [...prev, botMessage]);
        } catch (error) {
            setMessages(prev => [...prev, { sender: "bot", text: "Sorry, something went wrong." }]);
        }

        setInput("");
        setLoading(false);
    };

    return (
        <div className="mt-8 max-w-2xl mx-auto bg-white rounded-3xl shadow-2xl overflow-hidden border border-gray-100 transition-all duration-200">
            {/* Header */}
            <div className="bg-gradient-to-r from-blue-600 to-indigo-600 px-6 py-4 flex items-center gap-3">
                <div className="bg-white/20 p-2 rounded-full">
                    <ChatBubbleLeftRightIcon className="w-6 h-6 text-white" />
                </div>
                <div>
                    <h2 className="text-xl font-semibold text-white">Train Prediction Assistant</h2>
                    <p className="text-blue-100 text-sm">AI-powered availability & price trends</p>
                </div>
            </div>

            {/* Messages */}
            <div className="h-[480px] overflow-y-auto px-6 py-4 bg-gray-50/80 scroll-smooth">
                {messages.map((message, index) => (
                    <div
                        key={index}
                        className={`flex items-start gap-2.5 mb-4 ${message.sender === "user" ? "flex-row-reverse" : ""
                            }`}
                    >
                        {/* Avatar */}
                        <div
                            className={`w-8 h-8 rounded-full flex-shrink-0 flex items-center justify-center ${message.sender === "user"
                                ? "bg-blue-600 text-white"
                                : "bg-gray-300 text-gray-700"
                                }`}
                        >
                            {message.sender === "user" ? (
                                <UserIcon className="w-4 h-4" />
                            ) : (
                                <SparklesIcon className="w-4 h-4" />
                            )}
                        </div>

                        {/* Message bubble */}
                        <div
                            className={`max-w-[80%] px-4 py-2.5 rounded-2xl shadow-sm ${message.sender === "user"
                                ? "bg-blue-600 text-white rounded-tr-sm"
                                : "bg-white text-gray-800 rounded-tl-sm border border-gray-200"
                                }`}
                        >
                            <p className="whitespace-pre-wrap break-words text-sm leading-relaxed">
                                {message.text}
                            </p>
                        </div>
                    </div>
                ))}

                {/* Typing indicator */}
                {loading && (
                    <div className="flex items-start gap-2.5 mb-4">
                        <div className="w-8 h-8 rounded-full flex-shrink-0 bg-gray-300 flex items-center justify-center">
                            <SparklesIcon className="w-4 h-4 text-gray-700" />
                        </div>
                        <div className="bg-white px-4 py-2.5 rounded-2xl rounded-tl-sm border border-gray-200 shadow-sm">
                            <div className="flex gap-1">
                                <span className="w-2 h-2 bg-gray-400 rounded-full animate-bounce [animation-delay:-0.3s]"></span>
                                <span className="w-2 h-2 bg-gray-400 rounded-full animate-bounce [animation-delay:-0.15s]"></span>
                                <span className="w-2 h-2 bg-gray-400 rounded-full animate-bounce"></span>
                            </div>
                        </div>
                    </div>
                )}
                <div ref={messagesEndRef} />
            </div>

            {/* Input area */}
            <div className="border-t border-gray-200 bg-white px-4 py-3">
                <div className="flex items-center gap-2">
                    <input
                        className="flex-1 border border-gray-300 rounded-full px-5 py-2.5 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all placeholder-gray-400"
                        placeholder="Ask about train availability..."
                        value={input}
                        onChange={e => setInput(e.target.value)}
                        onKeyDown={e => {
                            if (e.key === "Enter" && !e.shiftKey) {
                                e.preventDefault();
                                handleSend();
                            }
                        }}
                        disabled={loading}
                    />
                    <button
                        onClick={handleSend}
                        disabled={loading || !input.trim()}
                        className={`px-5 py-2.5 rounded-full text-sm font-medium transition-all ${loading || !input.trim()
                            ? "bg-gray-200 text-gray-400 cursor-not-allowed"
                            : "bg-blue-600 text-white hover:bg-blue-700 active:scale-95 shadow-md hover:shadow-lg"
                            }`}
                    >
                        {loading ? "..." : "Send"}
                    </button>
                </div>
            </div>
        </div>
    );
}

export default Chatbot;