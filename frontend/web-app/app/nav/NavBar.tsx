import React from 'react'
import Search from './Search'
import Logo from './Logo'
import LoginButton from './LoginButton'
import { getCurrentUser } from '../actions/authAction'
import UserActions from './UserActions'

export default async function NavBar() {
    const user = await getCurrentUser();
    return (
        <header className='flex sticky top-0 z-50 justify-between 
        bg-white p-5 items-center text-gray-500 shadow-md'>
            <Logo />
            <Search />
            {user ? (
                <UserActions user={user}/>
            ) : (
                <LoginButton />
            )}
        </header>
    )
}
