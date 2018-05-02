from kivy.app import App
from kivy.uix.widget import Widget
from kivy.core.window import Window
from kivy.properties import NumericProperty, ReferenceListProperty,\
    ObjectProperty
from kivy.vector import Vector
from kivy.clock import Clock
from kivy.graphics import Rectangle
from kivy.uix.image import Image
from kivy.cache import Cache


class PongPaddle(Widget):
    score = NumericProperty(0)
    velocity_x = NumericProperty(0)
    velocity_y = NumericProperty(0)
    velocity = ReferenceListProperty(velocity_x, velocity_y)

    def move(self):
        self.pos = Vector(*self.velocity) + self.pos

    def bounce_ball(self, ball):
        if self.collide_widget(ball):
            vx, vy = ball.velocity
            vel = Vector(0, 0)
            ball.velocity = vel.x, vel.y
            if self.center_x > ball.center_x:
                mx = -1
            else:
                mx = 1
            if self.center_y + 150 > ball.center_y:
                ball.center_y = ball.center_y
            elif self.center_y - 150 < ball.center_y:
                ball.center_y = ball.center_y
            ball.center_x = self.center_x + (mx * 50)


class LinkImage(Image):
    source="./sprites/linkDown1.png"

    def move(self, ball):
        self.center_x = ball.center_x
        self.center_y = ball.center_y

    pass

class Background20(Image):
    source="./sprites/background20.png"
    pass

class Background21(Image):
    source="./sprites/background21.png"
    pass

class Background10(Image):
    source="./sprites/background10.png"
    pass

class Background11(Image):
    source="./sprites/background11.png"
    pass

class Background00(Image):
    source="./sprites/background00.png"
    pass

class Background01(Image):
    source="./sprites/background01.png"
    pass

class Background30(Image):
    source="./sprites/background30.png"
    pass

class Background31(Image):
    source="./sprites/background31.png"
    pass

class PongBall(Widget):
    velocity_x = NumericProperty(0)
    velocity_y = NumericProperty(0)
    velocity = ReferenceListProperty(velocity_x, velocity_y)

    def move(self):
        self.pos = Vector(*self.velocity) + self.pos


class PongGame(Widget):
    bkgr00 = ObjectProperty(None)
    bkgr01 = ObjectProperty(None)
    bkgr10 = ObjectProperty(None)
    bkgr11 = ObjectProperty(None)
    bkgr31 = ObjectProperty(None)
    bkgr30 = ObjectProperty(None)
    bkgr20 = ObjectProperty(None)
    bkgr21 = ObjectProperty(None)
    ball = ObjectProperty(None)
    link = ObjectProperty(None)
    seedText = ObjectProperty(None)
    mapIndx = 2
    mapIndy = 0

    def __init__(self, **kwargs):
        super(PongGame, self).__init__(**kwargs)
        self._keyboard = Window.request_keyboard(self._keyboard_closed, self)
        self._keyboard.bind(on_key_down=self._on_keyboard_down)
        self._keyboard.bind(on_key_up=self._on_keyboard_up)

    def _keyboard_closed(self):
        self._keyboard.unbind(on_key_down=self._on_keyboard_down)
        self._keyboard = None

    def seedUpdate(self):
        self.seedText = str(self.ball.center_x), str(self.ball.center_y), str(self.mapIndy), str(self.mapIndx*2)

    def _on_keyboard_down(self, keyboard, keycode, text, modifiers):
        if keycode[1] == 'w':
            self.ball.velocity_y = 10
        if keycode[1] == 's':
            self.ball.velocity_y = -10
        if keycode[1] == 'a':
            self.ball.velocity_x = -10
        if keycode[1] == 'd':
            self.ball.velocity_x = 10
        if keycode[1] == 'up':
            self.ball.velocity_y = 10
        if keycode[1] == 'down':
            self.ball.velocity_y = -10
        if keycode[1] == 'left':
            self.ball.velocity_x = -10
        if keycode[1] == 'right':
            self.ball.velocity_x = 10
        if keycode[1] == 'm':
            self.seedUpdate()
        return True

    def _on_keyboard_up(self, keyboard, keycode):
        if keycode[1] == 'w':
            self.ball.velocity_y = 0
        if keycode[1] == 's':
            self.ball.velocity_y = 0
        if keycode[1] == 'a':
            self.ball.velocity_x = 0
        if keycode[1] == 'd':
            self.ball.velocity_x = 0
        if keycode[1] == 'up':
            self.ball.velocity_y = 0
        if keycode[1] == 'down':
            self.ball.velocity_y = 0
        if keycode[1] == 'left':
            self.ball.velocity_x = 0
        if keycode[1] == 'right':
            self.ball.velocity_x = 0
        return True

    def serve_ball(self, vel=(0, 0)):
        self.seedText = ""
        self.ball.center = self.center
        self.ball.velocity = vel
        self.bkgr01.center_x=7500 
        self.bkgr00.center_x=7500 
        self.bkgr11.center_x=7500 
        self.bkgr21.center_x=7500 
        self.bkgr10.center_x=7500 
        self.bkgr30.center_x=7500 
        self.bkgr31.center_x=7500 


    def travel(self):
        if(self.mapIndx==0 and self.mapIndy==0):   
            self.bkgr00.center_x=800 
            self.bkgr01.center_x=7500 
            self.bkgr10.center_x=7500
        elif(self.mapIndx==0 and self.mapIndy==1):   
            self.bkgr01.center_x=800 
            self.bkgr00.center_x=7500 
            self.bkgr11.center_x=7500
        elif(self.mapIndx==1 and self.mapIndy==0):   
            self.bkgr10.center_x=800
            self.bkgr00.center_x=7500 
            self.bkgr01.center_x=7500 
            self.bkgr11.center_x=7500 
            self.bkgr20.center_x=7500
            self.bkgr21.center_x=7500
            self.bkgr30.center_x=7500
            self.bkgr31.center_x=7500
        elif(self.mapIndx==1 and self.mapIndy==1):   
            self.bkgr11.center_x=800
            self.bkgr01.center_x=7500 
            self.bkgr10.center_x=7500 
            self.bkgr21.center_x=7500
        elif(self.mapIndx==2 and self.mapIndy==0):   
            self.bkgr20.center_x=800
            self.bkgr10.center_x=7500 
            self.bkgr21.center_x=7500 
            self.bkgr30.center_x=7500
        elif(self.mapIndx==2 and self.mapIndy==1):   
            self.bkgr21.center_x=800
            self.bkgr11.center_x=7500 
            self.bkgr20.center_x=7500 
            self.bkgr31.center_x=7500
        elif(self.mapIndx==3 and self.mapIndy==0):   
            self.bkgr30.center_x=800
            self.bkgr20.center_x=7500 
            self.bkgr31.center_x=7500 
            self.bkgr21.center_x=7500
        elif(self.mapIndx==3 and self.mapIndy==1):   
            self.bkgr31.center_x=800
            self.bkgr21.center_x=7500 
            self.bkgr30.center_x=7500 

    def update(self, dt):
        self.ball.move()
        self.link.move(self.ball)

        if (self.ball.y < self.y):
            self.ball.velocity_y *= 0
            self.ball.center_y = self.top - 150
            self.mapIndy -= 1
            self.travel()

        if (self.ball.top > self.top - 100):
            self.ball.velocity_y *= 0
            self.ball.center_y = self.y + 50
            self.mapIndy += 1
            self.travel()

        if (self.ball.right > self.right):
            self.ball.velocity_x *= 0
            self.ball.center_x = self.x + 50
            self.mapIndx += 1
            self.travel()

        if (self.ball.x < self.x):
            self.ball.velocity_x *= 0
            self.ball.center_x = self.right - 50
            self.mapIndx -= 1
            self.travel()


class PongApp(App):
    def build(self):
        game = PongGame()
        game.serve_ball()
        Clock.schedule_interval(game.update, 1.0 / 60.0)
        return game


if __name__ == '__main__':
    PongApp().run()