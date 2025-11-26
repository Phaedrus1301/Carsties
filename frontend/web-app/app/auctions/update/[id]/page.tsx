import { getDetailedViewData } from '@/app/actions/auctionAction';
import Heading from '@/app/components/Heading';
import React from 'react'
import AuctionForm from '../../AuctionForm';

export default async function Update({params}: {params: {id: string}}) {
    const unwrappedParams = await params;
    const data = await getDetailedViewData(unwrappedParams.id);


    return (
        <div className='mx-auto max-w-75% shadow-lg p-10 bg-white rounded-lg'>
            <Heading title='Update your auction' subtitle='Please update details of your auction(only the following auction properties can be updated)' />
            <AuctionForm auction={data} />
        </div>
    )
}
