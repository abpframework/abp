export {};
declare global {
    interface Date {
        toLocalISOString(): string;
    }
}
