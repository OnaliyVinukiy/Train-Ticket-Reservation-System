export const RECURRENCE_PATTERNS = {
    None: "None",
    Daily: "Daily",
    Weekly: "Weekly",
    Monthly: "Monthly"
} as const;

export type RecurrencePattern = typeof RECURRENCE_PATTERNS[keyof typeof RECURRENCE_PATTERNS];