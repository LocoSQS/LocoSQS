import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageSenderComponent } from './message-sender.component';

describe('MessageSenderComponent', () => {
  let component: MessageSenderComponent;
  let fixture: ComponentFixture<MessageSenderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MessageSenderComponent]
    });
    fixture = TestBed.createComponent(MessageSenderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
