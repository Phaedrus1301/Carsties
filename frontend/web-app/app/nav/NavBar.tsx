import React from 'react'
import Search from './Search'
import Logo from './Logo'

export default function NavBar() {
  return (
    <header className='flex sticky top-0 z-50 justify-between 
    bg-white p-5 items-center text-gray-500 shadow-md'>
        <Logo />
        <Search />
        <div>Login</div>
    </header>
  )
}
