﻿using System;
using Scripts.UI.Models;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class CharacterSelectionController : MonoBehaviour
    {
        public event Action CharacterChosen;

        public event Action CharacterCancelled;

        [Header("Configuration"), SerializeField]
        private int characterNameLength;

        private CharacterSelectionWindow characterSelectionWindow;
        private CharacterNameWindow characterNameWindow;

        private void Awake()
        {
            CreateCharacterSelectionWindow();
            CreateCharacterNameWindow();
        }

        private void CreateCharacterSelectionWindow()
        {
            characterSelectionWindow = UIElementsCreator.GetInstance()
                .Create<CharacterSelectionWindow>();
            characterSelectionWindow.ChooseButtonClicked +=
                OnChooseButtonClicked;
            characterSelectionWindow.CancelButtonClicked +=
                OnCancelButtonClicked;
            characterSelectionWindow.CharacterSelected += 
                OnCharacterSelected;
        }

        private void CreateCharacterNameWindow()
        {
            characterNameWindow = UIElementsCreator.GetInstance()
                .Create<CharacterNameWindow>();
            characterNameWindow.ConfirmButtonClicked +=
                OnConfirmButtonClicked;
            characterNameWindow.BackButtonClicked += 
                OnBackButtonClicked;
            characterNameWindow.NameInputFieldChanged +=
                OnNameInputFieldChanged;
        }

        private void OnDestroy()
        {
            DestroyCharacterSelectionWindow();
            DestroyCharacterNameWindow();
        }

        private void DestroyCharacterSelectionWindow()
        {
            if (characterSelectionWindow != null)
            {
                characterSelectionWindow.ChooseButtonClicked -=
                    OnChooseButtonClicked;
                characterSelectionWindow.CancelButtonClicked -=
                    OnCancelButtonClicked;
                characterSelectionWindow.CharacterSelected -=
                    OnCharacterSelected;

                Destroy(characterSelectionWindow.gameObject);
            }
        }

        private void DestroyCharacterNameWindow()
        {
            if (characterNameWindow != null)
            {
                characterNameWindow.ConfirmButtonClicked -=
                    OnConfirmButtonClicked;
                characterNameWindow.BackButtonClicked -= 
                    OnBackButtonClicked;
                characterNameWindow.NameInputFieldChanged -=
                    OnNameInputFieldChanged;

                Destroy(characterNameWindow.gameObject);
            }
        }

        private void OnNameInputFieldChanged(string characterName)
        {
            if (characterName.Length >= characterNameLength)
            {
                if (characterNameWindow != null)
                {
                    characterNameWindow.EnableConfirmButton();
                }
            }
            else
            {
                if (characterNameWindow != null)
                {
                    characterNameWindow.DisableConfirmButton();
                }
            }
        }

        private void OnConfirmButtonClicked(string characterName)
        {
            HideCharacterNameWindow();

            CharacterDetails.GetInstance()
                .SetCharacterName(characterName);

            CharacterChosen?.Invoke();
        }

        private void OnBackButtonClicked()
        {
            HideCharacterNameWindow();
            ShowCharacterSelectionWindow();
        }

        private void OnChooseButtonClicked()
        {
            HideCharacterSelectionWindow();
            ShowCharacterNameWindow();
        }

        private void OnCancelButtonClicked()
        {
            HideCharacterSelectionWindow();

            CharacterCancelled?.Invoke();
        }

        private void OnCharacterSelected(UICharacterClass uiCharacterClass)
        {
            CharacterDetails.GetInstance()
                .SetCharacterClass(uiCharacterClass);
        }

        private void ShowCharacterNameWindow()
        {
            if (characterNameWindow != null)
            {
                characterNameWindow.Show();
            }
        }

        private void HideCharacterNameWindow()
        {
            if (characterNameWindow != null)
            {
                characterNameWindow.Hide();
            }
        }

        public void ShowCharacterSelectionWindow()
        {
            if (characterSelectionWindow != null)
            {
                characterSelectionWindow.Show();
            }
        }

        private void HideCharacterSelectionWindow()
        {
            if (characterSelectionWindow != null)
            {
                characterSelectionWindow.Hide();
            }
        }
    }
}