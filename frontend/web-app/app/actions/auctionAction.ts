'use server';
import { fetchWrapper } from "@/lib/fetchWrapper";
import { Auction, Bid, PagedResult } from "@/types";
import { FieldValues } from "react-hook-form";

export async function getData(query: string): Promise<PagedResult<Auction>> {
    return fetchWrapper.get(`search${query}`);
}

export async function updateAuctionTest(): Promise<{status: number, message: string}> {
    const data = {
        mileage: Math.floor(Math.random() * 1000) + 1
    }
    // `http://localhost:6001/auctions/dc1e4071-d19d-459b-b848-b5c3cd3d151f`
    return fetchWrapper.put('auctions/dc1e4071-d19d-459b-b848-b5c3cd3d151f', data);
}

export async function createAuction(data: FieldValues) {
    return fetchWrapper.post('auctions', data);
}

export async function deleteAuction(id: string) {
    return fetchWrapper.del(`auctions/${id}`);
}

export async function getDetailedViewData(id: string): Promise<Auction> {
    return fetchWrapper.get(`auctions/${id}`);
}

export async function updateAuction(data: FieldValues, id: string) {
    return fetchWrapper.put(`auctions/${id}`, data);
}

export async function getBidsForAuction(id: string): Promise<Bid[]> {
    return fetchWrapper.get(`bids/${id}`);
}

export async function placeBidForAuction(auctionId: string, amount: number) {
    return fetchWrapper.post(`bids?auctionId=${auctionId}&amount=${amount}`, {});
}