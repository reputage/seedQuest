

-------
Other
---------

https://github.com/philtoms/mithril-starter-kit

-------
Install
------

https://mithril.js.org/installation.html


Need to install NodeJs which installs npm

/Install/Install/Web/NodeJs/NodeJSNote.txt

Installed version 8.5 of nodejs and version 5.3.0 of npm

To use Mithril via NPM, go to your project folder, and run 
$ npm init --yes 

from the command line. 


$ cd /Data/Code/private/indigo/bluepea/src/bluepea/static

This will create a file called package.json.

$ npm init --yes

Wrote to /Data/Code/private/indigo/bluepea/src/bluepea/static/package.json:

{
  "name": "static",
  "version": "1.0.0",
  "description": "",
  "main": "main.js",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1"
  },
  "keywords": [],
  "author": "",
  "license": "ISC"
}

Edited package.json to be

{
  "name": "bluepea",
  "version": "1.0.0",
  "description": "",
  "main": "main.js",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1"
  },
  "keywords": [],
  "author": "",
  "license": "Apache2"
}


# creates a file called package.json
Then, to install Mithril, run:

$ npm install mithril --save

$ npm install mithril --save
npm notice created a lockfile as package-lock.json. You should commit this file.
npm WARN bluepea@1.0.0 No description
npm WARN bluepea@1.0.0 No repository field.
npm WARN bluepea@1.0.0 license should be a valid SPDX license expression

+ mithril@1.1.3
added 1 package in 1.583s

Edited package.json license to be valid

{
  "name": "bluepea",
  "version": "1.0.0",
  "description": "",
  "main": "main.js",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1"
  },
  "keywords": [],
  "author": "",
  "license": "Apache-2.0",
  "dependencies": {
    "mithril": "^1.1.3"
  }
}

Install added node_modules folder which has mithril folder

This will create a folder called node_modules, 
and a mithril folder inside of it. 
It will also add an entry under dependencies in the package.json file

You are now ready to start using Mithril. The recommended way to structure code 
is to modularize it via CommonJS modules:

Create subdirectory src/in the root of the static project directory
Create file  index.js in src
/Data/Code/private/indigo/bluepea/src/bluepea/static/src/index.js

// index.js
var m = require("mithril")

m.render(document.body, "hello world")

Modularization is the practice of separating the code into files. 
Doing so makes it easier to find code, understand what code relies on what code, 
and test.

CommonJS is a de-facto standard for modularizing Javascript code, 
and it's used by Node.js, as well as tools like Browserify and Webpack. 
It's a robust, battle-tested precursor to ES6 modules. 
Although the syntax for ES6 modules is specified in Ecmascript 6, 
the actual module loading mechanism is not. If you wish to use ES6 modules 
despite the non-standardized status of module loading, 
you can use tools like Rollup, Babel or Traceur.

CommonJS is a project with the goal of specifying an ecosystem for JavaScript 
outside the browser (for example, on the server or for native desktop applications).
https://webpack.github.io/docs/commonjs.html

CommonJS uses "module.exports" and "require" global functions


Most browser today do not natively support modularization systems (CommonJS or ES6), 
so modularized code must be bundled into a single Javascript file before 
running in a client-side application.

A popular way for creating a bundle is to setup an NPM script for Webpack. 
To install Webpack, run this from the command line:

$ npm install webpack --save-dev


> fsevents@1.1.2 install /Data/Code/private/indigo/bluepea/src/bluepea/static/node_modules/fsevents
> node install

[fsevents] Success: "/Data/Code/private/indigo/bluepea/src/bluepea/static/node_modules/fsevents/lib/binding/Release/node-v57-darwin-x64/fse.node" already installed
Pass --update-binary to reinstall or --build-from-source to recompile

> uglifyjs-webpack-plugin@0.4.6 postinstall /Data/Code/private/indigo/bluepea/src/bluepea/static/node_modules/uglifyjs-webpack-plugin
> node lib/post_install.js

npm WARN bluepea@1.0.0 No description
npm WARN bluepea@1.0.0 No repository field.

+ webpack@3.5.6
added 365 packages in 9.141s


Open the package.json that you created earlier, and add an entry to the scripts section:

{
    "name": "my-project",
    "scripts": {
        "start": "webpack src/index.js bin/app.js -d --watch"
    }
}
Remember this is a JSON file, so object key names such as "scripts" and "start" 
must be inside of double quotes.

The -d flag tells webpack to use development mode, which produces source maps 
for a better debugging experience.

The --watch flag tells webpack to watch the file system and automatically 
recreate app.js if file changes are detected.

Now you can run the script via npm start in your command line window. 
This looks up the webpack command in the NPM path, reads index.js and 
creates a file called app.js which includes both Mithril and the hello world code above. 
If you want to run the webpack command directly from the command line, 
you need to either add node_modules/.bin to your PATH, 
or install webpack globally via npm install webpack -g. 
It's, however, recommended that you always install webpack locally 
and use npm scripts, to ensure builds are reproducible in different computers.

$ npm start
or

$ npm run-script start

> bluepea@1.0.0 start /Data/Code/private/indigo/bluepea/src/bluepea/static
> webpack src/index.js bin/app.js -d --watch


Webpack is watching the files…

Hash: 6e54ba545ecac67c56f9
Version: webpack 3.5.6
Time: 204ms
 Asset    Size  Chunks             Chunk Names
app.js  171 kB       0  [emitted]  main
   [0] (webpack)/buildin/global.js 509 bytes {0} [built]
   [1] ./src/index.js 79 bytes {0} [built]
    + 4 hidden modules


Control-c to exit

The start command created the directory bin with 
bin/app.js in it.




$ ll
total 336
drwxrwxr-x   3 samuel  staff     102 Sep 14 16:24 ./
drwxrwxr-x  11 samuel  staff     374 Sep 14 16:22 ../
-rw-rw-r--   1 samuel  staff  171200 Sep 14 16:24 app.js


Now update main.html to reference the  bin/app.js as the packaged mithril load

<html>
  <head>
    <title>Hello world</title>
  </head>
  <body>
    <script src="bin/app.js"></script>
  </body>
</html>

This takes 5 seconds to load the app.js with the Valet server?
Tried changed from 1/16  0.0625  period to 0.015625 period 1/64 of the ioflo
skeddar but did not change the time in Safari

Using production build which is 10 times smaller did not speed it up.

On google chrome it loads right away.


To export a module, assign what you want to export to the special module.exports object:

// mycomponent.js
module.exports = {
    view: function() {return "hello from a module"}
}
In the index.js, you would then write this code to import that module:

// index.js
var m = require("mithril")

var MyComponent = require("./mycomponent")

m.mount(document.body, MyComponent)


Note that in this example, we're using m.mount, which wires up the component 
to Mithril's autoredraw system. 

In most applications, you will want to use m.mount 
(or m.route if your application has multiple screens) instead of m.render 
to take advantage of the autoredraw system, 
rather than re-rendering manually every time a change occurs.


Production build

If you open bin/app.js, you'll notice that the Webpack bundle is not minified, 
so this file is not ideal for a live application. 
To generate a minified file, open package.json and add a new npm script:

{
    "name": "my-project",
    "scripts": {
        "start": "webpack src/index.js bin/app.js -d --watch",
        "release": "webpack src/index.js bin/app.js -p",
    }
}

$ npm run-script build   # just npm build does not work

> bluepea@1.0.0 build /Data/Code/private/indigo/bluepea/src/bluepea/static
> webpack src/index.js bin/app.js -p

Hash: 4dabb1851dea9f53da7d
Version: webpack 3.5.6
Time: 563ms
 Asset     Size  Chunks             Chunk Names
app.js  27.8 kB       0  [emitted]  main
   [0] (webpack)/buildin/global.js 509 bytes {0} [built]
   [1] ./src/index.js 79 bytes {0} [built]
    + 4 hidden modules

$ ll bin
total 56
drwxrwxr-x   3 samuel  staff    102 Sep 14 16:49 ./
drwxrwxr-x  11 samuel  staff    374 Sep 14 16:22 ../
-rw-rw-r--   1 samuel  staff  27832 Sep 14 16:49 app.js



You can use hooks in your production environment to run the production build 
script automatically. Here's an example for Heroku:

{
    "name": "my-project",
    "scripts": {
        "start": "webpack -d --watch",
        "build": "webpack -p",
        "heroku-postbuild": "webpack -p"
    }
}


Alternate ways to use Mithril

Live reload development environment

Live reload is a feature where code changes automatically trigger the page to reload. Budo is one tool that enables live reloading.

# 1) install
npm install mithril --save
npm install budo -g

# 2) add this line into the scripts section in package.json
#    "scripts": {
#        "start": "budo --live --open index.js"
#    }

# 3) create an `index.js` file

# 4) run budo
npm start
The source file index.js will be compiled (bundled) and a browser window opens showing the result. Any changes in the source files will instantly get recompiled and the browser will refresh reflecting the changes.

Mithril bundler

Mithril comes with a bundler tool of its own. It is sufficient for ES5-based projects that have no other dependencies other than Mithril, but it's currently considered experimental for projects that require other NPM dependencies. It produces smaller bundles than webpack, but you should not use it in production yet.

If you want to try it and give feedback, you can open package.json and change the npm script for webpack to this:

{
    "name": "my-project",
    "scripts": {
        "build": "bundle index.js --output app.js --watch"
    }
}
Vanilla

If you don't have the ability to run a bundler script due to company security policies, there's an options to not use a module system at all:

<html>
  <head>
    <title>Hello world</title>
  </head>
  <body>
    <script src="https://cdn.rawgit.com/MithrilJS/mithril.js/master/mithril.js"></script>
    <script src="index.js"></script>
  </body>
</html>
// index.js

// if a CommonJS environment is not detected, Mithril will be created in the global scope
m.render(document.body, "hello world")


----------
Test Framework
------------
Mithril comes with a testing framework called ospec. 
What makes it different from most test frameworks is that it avoids all 
configurability for the sake of avoiding yak shaving and analysis paralysis.

The easist way to setup the test runner is to create an NPM script for it. 
Open your project's package.json file and edit the test line under the scripts section:

{
    "name": "my-project",
    "scripts": {
        "test": "ospec"
    }
}
Remember this is a JSON file, so object key names such as "test" must be inside of double quotes.

To setup a test suite, create a tests folder and inside of it, create a test file:

Made new directior static/tests


// file: tests/math-test.js
var o = require("mithril/ospec/ospec")

o.spec("math", function() {
    o("addition works", function() {
        o(1 + 2).equals(3)
    })
})
To run the test, use the command npm test. Ospec considers any Javascript file 
inside of a tests folder (anywhere in the project) to be a test.

npm test


$ npm test

> bluepea@1.0.0 test /Data/Code/private/indigo/bluepea/src/bluepea/static
> ospec

0 assertions completed in 0ms, of which 0 failed
samuel@AiBook:/Data/Code/private/indigo/bluepea/src/bluepea/static/


-----------
Semantic UI
-----------

http://noeticforce.com/css-front-end-frameworks-for-web-development-and-design
https://semantic-ui.com/introduction/getting-started.html

Install NodeJS

Update NPM

$ npm update
$ npm i -g npm


---------
Install Gulp
--------------

Install Gulp globally
https://github.com/gulpjs/gulp/blob/master/docs/getting-started.md
https://medium.com/gulpjs/gulp-sips-command-line-interface-e53411d4467

Traditionally, you’ve run your tasks using the gulp command installed 
by the main gulp package on npm. However, we’ve moved away from coupling 
the CLI and library together. The CLI now lives in the gulp-cli package.

Install global gulp-cli

$ npm install --global gulp-cli

cd to static directory to install local

$ npm install --save-dev gulp

$ gulp -v
$ gulp --version
[15:03:52] CLI version 1.4.0
[15:03:52] Local version 3.9.1

In your project directory, create a file named gulpfile.js in your project root with these contents:

var gulp = require('gulp');

gulp.task('default', function() {
  // place code for your default task here
});
Test it out

Run the gulp command in your project directory:

$ gulp
[15:05:30] Using gulpfile /Data/Code/private/indigo/bluepea/src/bluepea/static/gulpfile.js
[15:05:30] Starting 'default'...
[15:05:30] Finished 'default' after 67 μs


---------
Install Semantic UI
------------

Go to root of project directory

$ npm install semantic-ui --save

Installing
------------------------------
Installing to semantic/
Copying UI definitions
Copying UI themes
Copying gulp tasks
Adding theme files
Creating gulpfile.js
Creating site theme folder /Data/Code/private/indigo/bluepea/src/bluepea/static/semantic/src/site/
[15:10:47] Starting 'create theme.config'...
Adjusting @siteFolder to:  site/
Creating src/theme.config (LESS config) /Data/Code/private/indigo/bluepea/src/bluepea/static/semantic/src/theme.config
[15:10:47] Finished 'create theme.config' after 13 ms
[15:10:47] Starting 'create semantic.json'...
Creating config file (semantic.json) /Data/Code/private/indigo/bluepea/src/bluepea/static/semantic.json
[15:10:47] Finished 'create semantic.json' after 11 ms
[15:10:47] Finished 'create install files' after 187 ms
[15:10:47] Starting 'clean up install'...

 Setup Complete! 
 Installing Peer Dependencies. Please refrain from ctrl + c... 
 After completion navigate to semantic/ and run "gulp build" to build
npm WARN bluepea@1.0.0 No description
npm WARN bluepea@1.0.0 No repository field.

+ semantic-ui@2.2.13
added 321 packages in 51.477s


$ cd semantic/
$ gulp build

[15:11:21] Using gulpfile /Data/Code/private/indigo/bluepea/src/bluepea/static/semantic/gulpfile.js
[15:11:21] Starting 'build'...
Building Semantic
[15:11:21] Starting 'build-javascript'...
Building Javascript
[15:11:21] Starting 'build-css'...
Building CSS
[15:11:21] Starting 'build-assets'...
Building assets
[15:11:22] Created: dist/components/site.js
...
[15:11:25] Created: dist/components/state.min.js
[15:11:25] Finished 'build-assets' after 4.02 s
[15:11:25] Created: dist/components/visibility.min.js
[15:11:25] Starting 'package compressed js'...
[15:11:25] Starting 'package uncompressed js'...
[15:11:25] Finished 'build-javascript' after 4.06 s
[15:11:25] Created: dist/components/container.css
...
[15:11:28] Created: dist/semantic.min.js
[15:11:28] Finished 'package compressed js' after 2.25 s
[15:11:28] Created: dist/semantic.js
[15:11:28] Finished 'package uncompressed js' after 2.25 s
[15:11:28] Created: dist/components/flag.css
...
[15:11:31] Created: dist/components/transition.min.css
[15:11:31] Starting 'package compressed css'...
[15:11:31] Created: dist/components/transition.css
[15:11:31] Starting 'package uncompressed css'...
[15:11:36] Created: dist/semantic.min.css
[15:11:36] Finished 'package compressed css' after 4.87 s
[15:11:36] Created: dist/semantic.css
[15:11:36] Finished 'package uncompressed css' after 4.74 s
[15:11:36] Finished 'build-css' after 15 s
[15:11:36] Finished 'build' after 15 s

Updating

Updating via NPM
Semantic's NPM install script will automatically update Semantic UI to the latest version while preserving your site and packaged themes.

$ npm update



Install jquery locally using npm

$ npm install jquery --save

Browserify/Webpack
There are several ways to use Browserify and Webpack. 
For more information on using these tools, 
please refer to the corresponding project's documention. 
In the script, including jQuery will usually look like this...

var $ = require("jquery");

--------------------
Include in Your HTML
-------------------
------
local jquery
---------

<html>
  <head>
    <link rel="stylesheet" type="text/css" href="semantic/dist/semantic.min.css">
    <script src="node_modules/jquery/dist/jquery.min.js"></script>
    <script src="semantic/dist/semantic.min.js"></script>
    <title>Hello world</title>
  </head>
  <body>
    <script src="bin/app.js"></script>
  </body>
</html>



-----
CDN jquery
-----

Running the gulp build tools will compile CSS and Javascript for use in your project. 
Just link to these files in your HTML along with the latest jQuery.

<link rel="stylesheet" type="text/css" href="semantic/dist/semantic.min.css">
<script
  src="https://code.jquery.com/jquery-3.1.1.min.js"
  integrity="sha256-hVVnYaiADRTO2PzUGmuLJr8BLUSjGIZsDYGmIJLv2b8="
  crossorigin="anonymous"></script>
<script src="semantic/dist/semantic.min.js"></script>



--------------
Transcrypt
------------

http://www.transcrypt.org

$ pip3 install -U transcrypt

Created static/transcrypt/ subdirectory to hold transcrypt python files


Contents of hello.py

m = require("mithril")

m.render(document.body, "Hello python")


If you want to include Python code that makes full use of generators, iterators and the yield statement, the following workflow is advised:

Initially compile your code using the switches: -b -m -e 6 -n.
Debug your non-minified code in a JavaScript 6 compatible browser like Google Chrome. 
Both .js and the .py files will be human readable. 
The sourcemap will refer from the non-minified JavaScript target code to the 
Python source code, allowing you to debug both in Python and in JavaScript.
If it all works, compile your code using the switches -b -m and distribute 
the minified version. It will run in any JavaScript 5 compatible browser. 
Python source level debugging is still possible since the sourcemap will refer 
from the minified JavaScript target code to the Python source code.

None minifiled

$ transcrypt -b -m -n  hello.py

Transcrypt (TM) Python to JavaScript Small Sane Subset Transpiler Version 3.6.49
Copyright (C) Geatec Engineering. License: Apache 2.0

Saving result in: /Data/Code/private/indigo/bluepea/src/bluepea/static/transcrypt/__javascript__/hello.js




main.html

Setup for transcrypt is

static/
  main.py
  __javascript__/
    main.js
    main.mod.js
    extra/
      sourcemap/
        main.js.map
        main.mod.js.map
  pylib/
    __init__.py
    hello.py
    __javascript__/
      hello.js
      hello.mod.js
      extra/
        sourcemap/
          hello.js.map
          hello.mod.js.map
          pylib.hello.mod.js.map

To compile python to js

$ transcrypt -b -m -n -e 6 main.py
