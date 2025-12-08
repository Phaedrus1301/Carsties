import Heading from '@/app/components/Heading'
import React from 'react'
import AuctionForm from '../AuctionForm'

export const dynamic = 'force-dynamic'

export default function Create() {
    return (
        <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
            <Heading title='Sell your car!' subtitle='please enter details of your car' />
            <AuctionForm />
        </div>
    )
}
