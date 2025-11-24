'use client';

import { useParamsStore } from '@/hooks/useParamsStore';
import React, { ChangeEvent, useEffect, useState } from 'react'
import { FaSearch } from 'react-icons/fa'

export default function Search() {
    const setParams = useParamsStore(state => state.setParams);
    const searchTerm = useParamsStore(state => state.searchTerm);
    const [value, setValue] = useState('');

    /*linter is shouting a warning for setValue
      there seems to be better option where u skip local state all in all
      HOWEVER, its updating the result super fast and you dont even need to wait to enter stuff
      i like fast but idk if thats good practice.
      another option is to use Zustand sub/unsub properly - guess we try in future once.
    */
    useEffect(() => {
        if(searchTerm === '') setValue('');
    }, [searchTerm]);

    function handleChange(e: ChangeEvent<HTMLInputElement>) {
        setValue(e.target.value);
    }

    function handleSearch() {
        setParams({searchTerm: value});
    }

    return (
        <div className='flex w-[50%] items-center border-2 border-gray-300 rounded-full py-2 shadow-sm'>
            <input
            onKeyDown={(e) => {
                if(e.key === 'Enter') {
                    handleSearch();
                }
            }}
            onChange={handleChange}
            value={value} 
            type="text"
            placeholder='Search for car by make, model or color'
            className="
                grow
                pl-5
                bg-transparent
                focus:outline-none
                border-transparent
                focus:border-transparent
                text-sm
                text-gray-600
            "
            />
            <button onClick={handleSearch}>
                <FaSearch size={34}
                    className='bg-red-400 text-white rounded-full p-2 cursor-pointer mx-2' />
            </button>
        </div>
    )
}
