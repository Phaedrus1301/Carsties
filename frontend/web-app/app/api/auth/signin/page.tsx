import EmptyFilter from '@/app/components/EmptyFilter'
import React from 'react'

export default async function SignIn({searchParams} : {searchParams: {callbackUrl: string}}) {
    const unwrappedSearchParams = await searchParams;
    const myUrl = unwrappedSearchParams.callbackUrl;

    return (
        <EmptyFilter
            title='You need to be logged in to do that'
            subtitle='Please click below to login'
            showLogin
            callbackUrl={myUrl}
        />
    )
}
