import {ErrorResponse} from "common/models/ErrorResponse";

export function isFetchBaseQueryError(
    error: unknown
): error is { status: number, data: ErrorResponse } {
    return typeof error === "object" && error != null && "status" in error;
}

export function isErrorWithMessage(
    error: unknown
): error is { message: string } {
    return (
        typeof error === "object" &&
        error != null &&
        "message" in error &&
        typeof (error as any).message === "string"
    );
}